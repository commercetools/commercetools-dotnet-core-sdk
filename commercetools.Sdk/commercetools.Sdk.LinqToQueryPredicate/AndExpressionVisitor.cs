using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class AndExpressionVisitor
    {
        private string left;
        private string right;

        public AndExpressionVisitor(BinaryExpression expression)
        {
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            left = queryPredicateExpressionVisitor.VisitExpression(expression.Left);
            right = queryPredicateExpressionVisitor.VisitExpression(expression.Right);
        }

        public override string ToString()
        {
            // TODO Combine parents
            // var expression 1 
            // var expression 2
            // if can combine 
            // var combined expression to string (parentslist(prop and prop)
            // else var express1 and express2
            return $"{left} and {right}";
        }
    }
}
