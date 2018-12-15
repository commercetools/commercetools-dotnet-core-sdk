using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class ComparisonPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

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

            IPredicateVisitor left = predicateVisitorFactory.Create(binaryExpression.Left);
            IPredicateVisitor right = predicateVisitorFactory.Create(binaryExpression.Right);
            string operatorSign = Mapping.GetOperator(expression.NodeType, Mapping.ComparisonOperators);

            // Predicates might need to be rearranged so that the brackets are rendered in a correct way
            return CanBeCombined(left) ? CombinePredicates(left, operatorSign, right) : new BinaryPredicateVisitor(left, operatorSign, right);
        }

        // When there is more than one property accessor, the last accessor needs to be taken out and added to the binary logical predicate.
        // property(property(property operator value)
        private static IPredicateVisitor CombinePredicates(IPredicateVisitor left, string operatorSign, IPredicateVisitor right)
        {
            var container = (ContainerPredicateVisitor)left;
            var parent = (ContainerPredicateVisitor)container.Parent;
            IPredicateVisitor innerLeft = container.Inner;
            BinaryPredicateVisitor binaryPredicateVisitor = new BinaryPredicateVisitor(innerLeft, operatorSign, right);
            ContainerPredicateVisitor binaryContainer = new ContainerPredicateVisitor(binaryPredicateVisitor, parent.Inner);
            ContainerPredicateVisitor combinedContainer = new ContainerPredicateVisitor(binaryContainer, parent.Parent);
            return combinedContainer;
        }

        private static bool CanBeCombined(IPredicateVisitor left)
        {
            return left is ContainerPredicateVisitor container && container.Parent is ContainerPredicateVisitor;
        }
    }
}
