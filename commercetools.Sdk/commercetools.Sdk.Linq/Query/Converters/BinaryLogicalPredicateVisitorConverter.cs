using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class BinaryLogicalPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return Mapping.LogicalOperators.ContainsKey(expression.NodeType);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            string operatorSign = Mapping.GetOperator(expression.NodeType, Mapping.LogicalOperators);
            IPredicateVisitor predicateVisitorLeft = predicateVisitorFactory.Create(binaryExpression.Left);
            IPredicateVisitor predicateVisitorRight = predicateVisitorFactory.Create(binaryExpression.Right);
            if (CanCombinePredicateVisitors(predicateVisitorLeft, predicateVisitorRight))
            {
                return CombinePredicateVisitors(predicateVisitorLeft, operatorSign, predicateVisitorRight);
            }

            return new BinaryPredicateVisitor(predicateVisitorLeft, operatorSign, predicateVisitorRight);
        }

        private static bool CanCombinePredicateVisitors(IPredicateVisitor left, IPredicateVisitor right)
        {
            return HasSameParents(left, right);
        }

        private static bool HasSameParents(IPredicateVisitor left, IPredicateVisitor right)
        {
            bool result = false;
            while (left is ContainerPredicateVisitor containerLeft && right is ContainerPredicateVisitor containerRight)
            {
                result = ArePropertiesEqual(containerLeft, containerRight);
                left = containerLeft.Inner;
                right = containerRight.Inner;
            }

            return result;
        }

        private static bool ArePropertiesEqual(ContainerPredicateVisitor left, ContainerPredicateVisitor right)
        {
            ConstantPredicateVisitor constantLeft = left.Parent as ConstantPredicateVisitor;
            ConstantPredicateVisitor constantRight = right.Parent as ConstantPredicateVisitor;
            if (constantLeft != null && constantRight != null)
            {
                return constantLeft.Constant == constantRight.Constant;
            }

            return false;
        }

        private static IPredicateVisitor CombinePredicateVisitors(IPredicateVisitor left, string operatorSign, IPredicateVisitor right)
        {
            IPredicateVisitor innerLeft = left;
            IPredicateVisitor innerRight = right;
            ContainerPredicateVisitor container = null;
            while (innerLeft is ContainerPredicateVisitor containerLeft && innerRight is ContainerPredicateVisitor containerRight)
            {
                container = containerLeft;
                innerLeft = containerLeft.Inner;
                innerRight = containerRight.Inner;
            }

            if (innerLeft is BinaryPredicateVisitor binaryLeft && innerRight is BinaryPredicateVisitor binaryRight)
            {
                BinaryPredicateVisitor binaryPredicateVisitor = new BinaryPredicateVisitor(innerLeft, operatorSign, innerRight);
                ContainerPredicateVisitor combined = new ContainerPredicateVisitor(binaryPredicateVisitor, container?.Parent);
                return combined;
            }

            return null;
        }
    }
}
