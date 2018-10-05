using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class NotExpressionVisitor
    {
        private string operand;

        public NotExpressionVisitor(UnaryExpression expression)
        {
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            operand = queryPredicateExpressionVisitor.VisitExpression(expression.Operand);
        }

        public override string ToString()
        {
            // TODO Combine parents
            return $"not({operand})";
        }
    }
}
