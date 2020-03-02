using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;
using commercetools.Sdk.Linq.Query;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class ValueOfEnumPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        private const string MethodName = "valueOfEnum";

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
            if (dynamicInvoke is Enum enumResult)
            {
                return new ConstantPredicateVisitor(enumResult.GetDescription().WrapInQuotes());
            }

            var compiledValue = dynamicInvoke.ToString();
            return new ConstantPredicateVisitor(compiledValue);
        }
    }
}
