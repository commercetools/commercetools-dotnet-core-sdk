using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class SimplePredicateVisitorFactory : ISimplePredicateVisitorFactory
    {
        private IAccessorTraverser accessorTraverser;

        public SimplePredicateVisitorFactory(IAccessorTraverser accessorTraverser)
        {
            this.accessorTraverser = accessorTraverser;
        }

        public SimplePredicateVisitor Create(BinaryExpression expression)
        {
            string left = GetLeft(expression.Left);
            string operatorSign = GetOperator(expression.NodeType);
            string right = GetRight(expression.Right);
            SimplePredicateVisitor simplePredicateVisitor = new SimplePredicateVisitor(left, operatorSign, right);
            return simplePredicateVisitor;
        }

        private string GetLeft(Expression expression)
        {
            return null;
        }

        private string GetRight(Expression expression)
        {
            return null;
        }

        private string GetOperator(ExpressionType expressionType)
        {
            return null;
        }
    }
}
