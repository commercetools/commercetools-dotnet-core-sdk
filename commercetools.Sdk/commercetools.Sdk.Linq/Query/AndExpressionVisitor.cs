using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class AndExpressionVisitor : Visitor
    {
        private Visitor left;
        private Visitor right;

        public AndExpressionVisitor(BinaryExpression expression)
        {
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            left = queryPredicateExpressionVisitor.VisitExpression(expression.Left);
            right = queryPredicateExpressionVisitor.VisitExpression(expression.Right);
        }

        public override string ToString()
        {
            if (CanBeCombined())
            {
                List<string> parentList = ((BinaryExpressionVisitor)left).parentList;
                // TODO dirty hack, find a better way to combined objects
                ((BinaryExpressionVisitor)left).parentList = new List<string>();
                ((BinaryExpressionVisitor)right).parentList = new List<string>();
                var result = $"{left.ToString()} and {right.ToString()}";
                return QueryPredicateExpressionVisitor.Visit(result, parentList);
            }
            return $"{left.ToString()} and {right.ToString()}";
        }

        private bool CanBeCombined()
        {
            bool isCorrectType = left.GetType().IsSubclassOf(typeof(BinaryExpressionVisitor)) && right.GetType().IsSubclassOf(typeof(BinaryExpressionVisitor));
            bool hasSameParents = isCorrectType && ((BinaryExpressionVisitor)left).parentList.SequenceEqual(((BinaryExpressionVisitor)right).parentList);
            return hasSameParents;
        }
    }
}