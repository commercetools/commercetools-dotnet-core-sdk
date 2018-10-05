using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class OrExpressionVisitor
    {
        private string left;
        private string right;

        public OrExpressionVisitor(BinaryExpression expression)
        {
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            left = queryPredicateExpressionVisitor.VisitExpression(expression.Left);
            right = queryPredicateExpressionVisitor.VisitExpression(expression.Right);
        }

        public override string ToString()
        {
            // TODO Combine parents
            return $"{left} or {right}";
        }
    }
}
