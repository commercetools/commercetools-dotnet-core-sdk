using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;


namespace commercetools.Sdk.Linq.Query.Converters
{
    public class CustomFieldsPredicateVisitorConverter : IQueryPredicateVisitorConverter
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
                    ContainerPredicateVisitor parentAccessor = predicateVisitorFactory.Create(memberExpression) as ContainerPredicateVisitor;
                    IPredicateVisitor constantPredicateVisitor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                    return new ContainerPredicateVisitor(RemoveQuotes(constantPredicateVisitor), parentAccessor);
                }
            }

            return null;
        }

        private static IPredicateVisitor RemoveQuotes(IPredicateVisitor inner)
        {
            if (inner is ConstantPredicateVisitor constantVisitor)
            {
                ConstantPredicateVisitor constantWithoutQuotes = new ConstantPredicateVisitor(constantVisitor.Constant.RemoveQuotes(), true);
                return constantWithoutQuotes;
            }

            return inner;
        }
    }
}