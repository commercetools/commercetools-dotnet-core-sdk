using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class CastMethodsPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name.StartsWith(Mapping.CastMethodStart, StringComparison.InvariantCulture))
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Object != null)
                {
                    return predicateVisitorFactory.Create(methodCallExpression.Object);
                }

                return predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            }

            return null;
        }
    }
}