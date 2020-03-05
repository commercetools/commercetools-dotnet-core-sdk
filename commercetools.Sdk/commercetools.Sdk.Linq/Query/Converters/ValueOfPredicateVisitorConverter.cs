using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;


namespace commercetools.Sdk.Linq.Query.Converters
{
    /// <summary>
    /// For comparison of non local values like (c.key == category.key.valueOf()) or (c.createdAt == date.valueOf())
    /// </summary>
    public class ValueOfPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        private const string MethodName = "valueOf";

        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.Name == MethodName)
            {
                return true;
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            var dynamicInvoke = Expression.Lambda(expression, null).Compile().DynamicInvoke(null);
            if (dynamicInvoke is DateTime dt)
            {
                return new ConstantPredicateVisitor(dt.ToUtcIso8601().WrapInQuotes());
            }

            if (dynamicInvoke is Enum enumResult)
            {
                return new ConstantPredicateVisitor(enumResult.GetDescription().WrapInQuotes());
            }

            if (dynamicInvoke.GetType().IsListOrArray())
            {
                var array = dynamicInvoke as IEnumerable;
                var predicateVisitors = new List<IPredicateVisitor>();
                if (array != null)
                {
                    foreach (var item in array)
                    {
                        var visitor = new ConstantPredicateVisitor(item.ToString().WrapInQuotes());
                        predicateVisitors.Add(visitor);
                    }
                }

                return new CollectionPredicateVisitor(predicateVisitors);
            }

            var compiledValue = dynamicInvoke.ToString();
            if ((expression as MethodCallExpression)?.Arguments[0].Type == typeof(string))
            {
                compiledValue = compiledValue.WrapInQuotes();
            }

            return new ConstantPredicateVisitor(compiledValue);
        }
    }
}
