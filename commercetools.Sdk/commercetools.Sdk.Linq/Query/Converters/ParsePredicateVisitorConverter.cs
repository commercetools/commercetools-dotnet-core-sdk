using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class ParsePredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        private const string MethodName = "Parse";

        public int Priority => 3;

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
