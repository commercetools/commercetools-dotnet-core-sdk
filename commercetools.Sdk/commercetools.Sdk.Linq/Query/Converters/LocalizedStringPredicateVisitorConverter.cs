using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class LocalizedStringPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Call;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            IPredicateVisitor inner = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            IPredicateVisitor innerWithoutQuotes = RemoveQuotes(inner);
            IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Object);
            ContainerPredicateVisitor container = new ContainerPredicateVisitor(innerWithoutQuotes, parent);
            return container;
        }

        private static IPredicateVisitor RemoveQuotes(IPredicateVisitor inner)
        {
            if (inner is ConstantPredicateVisitor constantVisitor)
            {
                ConstantPredicateVisitor constantWithoutQuotes = new ConstantPredicateVisitor(constantVisitor.Constant.RemoveQuotes());
                return constantWithoutQuotes;
            }

            return inner;
        }
    }
}
