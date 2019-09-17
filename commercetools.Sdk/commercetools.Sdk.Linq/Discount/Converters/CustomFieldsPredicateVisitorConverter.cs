using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class CustomFieldsPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "get_Item")
                {
                    if (methodCallExpression.Object is MemberExpression memberExpression)
                    {
                        if (memberExpression.Member.Name == "Fields")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Object is MemberExpression memberExpression)
                {
                    AccessorPredicateVisitor parentAccessor = predicateVisitorFactory.Create(memberExpression.Expression) as AccessorPredicateVisitor;
                    IPredicateVisitor constantPredicateVisitor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                    return new AccessorPredicateVisitor(RemoveQuotes(constantPredicateVisitor), parentAccessor);
                }
            }

            return null;
        }

        private static IPredicateVisitor RemoveQuotes(IPredicateVisitor inner)
        {
            if (inner is ConstantPredicateVisitor constantVisitor)
            {
                ConstantPredicateVisitor constantWithoutQuotes = new ConstantPredicateVisitor(constantVisitor.Constant.RemoveQuotes().WrapInBackticksIfNeeded(), true);
                return constantWithoutQuotes;
            }

            return inner;
        }
    }
}