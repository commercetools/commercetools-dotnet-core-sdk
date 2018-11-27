using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class NotExpressionVisitor : Visitor
    {
        private Visitor operand;

        public NotExpressionVisitor(UnaryExpression expression)
        {
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            operand = queryPredicateExpressionVisitor.VisitExpression(expression.Operand);
        }

        public override string ToString()
        {
            // TODO Combine parents
            return $"not({operand.ToString()})";
        }
    }
}