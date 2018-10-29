using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class LogicalPredicateVisitorFactory : ILogicalPredicateVisitorFactory
    {
        public LogicalPredicateVisitor Create(BinaryExpression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            ICartPredicateVisitor cartPredicateVisitorLeft = cartPredicateVisitorFactory.Create(expression.Left);
            ICartPredicateVisitor cartPredicateVisitorRight = cartPredicateVisitorFactory.Create(expression.Right);
            string operatorSign = GetOperator(expression.NodeType);
            if (operatorSign == LogicalOperators.AND)
            {
               return new LogicalPredicateVisitor(cartPredicateVisitorLeft, LogicalOperators.AND, cartPredicateVisitorRight);
            }
            if (operatorSign == LogicalOperators.OR)
            {
                return new LogicalPredicateVisitor(cartPredicateVisitorLeft, LogicalOperators.OR, cartPredicateVisitorRight);
            }
            throw new NotSupportedException();
        }

        private string GetOperator(ExpressionType expressionType)
        {
            return null;
        }
    }
}
