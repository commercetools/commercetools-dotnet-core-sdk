using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    // Not((c.Key == "c14"))
    // !(c.Key == "c14")
    public class NotLogicalPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Not;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            UnaryExpression unaryExpression = expression as UnaryExpression;
            if (unaryExpression == null)
            {
                return null;
            }

            // c.Key == "c14"
            IPredicateVisitor inner = predicateVisitorFactory.Create(unaryExpression.Operand);
            ConstantPredicateVisitor not = new ConstantPredicateVisitor(Mapping.Not);
            return new ContainerPredicateVisitor(inner, not);
        }
    }
}
