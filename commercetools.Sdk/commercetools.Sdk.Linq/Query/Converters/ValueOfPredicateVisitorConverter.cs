using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

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

            var compiledValue = dynamicInvoke.ToString();
            if ((expression as MethodCallExpression)?.Arguments[0].Type == typeof(string))
            {
                compiledValue = compiledValue.WrapInQuotes();
            }

            return new ConstantPredicateVisitor(compiledValue);
        }
    }
}
