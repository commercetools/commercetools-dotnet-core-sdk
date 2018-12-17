using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class ParsePredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (IsMatchingMethodName(methodCallExpression))
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            return predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
        }

        private static bool IsMatchingMethodName(MethodCallExpression methodCallExpression)
        {
            return Mapping.ParseMethods.Contains(methodCallExpression.Method.Name);
        }
    }
}