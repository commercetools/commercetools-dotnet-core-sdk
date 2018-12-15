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

            IPredicateVisitor predicateVisitorLeft = predicateVisitorFactory.Create(binaryExpression.Left);
            IPredicateVisitor predicateVisitorRight = predicateVisitorFactory.Create(binaryExpression.Right);
            if (CanCombinePredicateVisitors(predicateVisitorLeft, predicateVisitorRight))
            {
                CombinePredicateVisitors(predicateVisitorLeft, predicateVisitorRight);
            }

            string operatorSign = Mapping.GetOperator(expression.NodeType, Mapping.LogicalOperators);
            return new BinaryPredicateVisitor(predicateVisitorLeft, operatorSign, predicateVisitorRight);
        }

        private static bool CanCombinePredicateVisitors(IPredicateVisitor left, IPredicateVisitor right)
        {
            return false;
        }

        private static void CombinePredicateVisitors(IPredicateVisitor left, IPredicateVisitor right)
        {

        }
    }
}
