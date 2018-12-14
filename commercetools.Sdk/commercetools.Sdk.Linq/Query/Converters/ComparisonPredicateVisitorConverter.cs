using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class ComparisonPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            return Mapping.ComparisonOperators.ContainsKey(expression.NodeType);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            IPredicateVisitor predicateVisitorLeft = predicateVisitorFactory.Create(binaryExpression.Left);
            IPredicateVisitor predicateVisitorRight = predicateVisitorFactory.Create(binaryExpression.Right);
            string operatorSign = Mapping.GetOperator(expression.NodeType, Mapping.ComparisonOperators);
            if (predicateVisitorLeft is ContainerPredicateVisitor visitor)
            {
                IPredicateVisitor innerLeft = visitor.Inner;
                BinaryLogicalPredicateVisitor binaryLogicalPredicateVisitor = new BinaryLogicalPredicateVisitor(innerLeft, operatorSign, predicateVisitorRight);
                ContainerPredicateVisitor container = new ContainerPredicateVisitor(binaryLogicalPredicateVisitor, visitor.Parent);
                return container;
            }

            return new BinaryLogicalPredicateVisitor(predicateVisitorLeft, operatorSign, predicateVisitorRight);
        }
    }
}
