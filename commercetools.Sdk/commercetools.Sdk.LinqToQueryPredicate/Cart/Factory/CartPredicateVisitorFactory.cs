using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class CartPredicateVisitorFactory : ICartPredicateVisitorFactory
    {
        private readonly ILogicalPredicateVisitorFactory logicalPredicateVisitorFactory;

        public CartPredicateVisitorFactory(ILogicalPredicateVisitorFactory logicalPredicateVisitorFactory)
        {
            this.logicalPredicateVisitorFactory = logicalPredicateVisitorFactory;
        }

        public ICartPredicateVisitor Create(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return this.Create(((LambdaExpression)expression).Body);
            }
            var allowedLogicalOperators = new List<ExpressionType>() { ExpressionType.And, ExpressionType.AndAlso, ExpressionType.Or, ExpressionType.OrElse };
            if (allowedLogicalOperators.Contains(expression.NodeType))
            {
                return this.logicalPredicateVisitorFactory.Create((BinaryExpression)expression, this);
            }
            var allowedOperators = new List<ExpressionType>() { ExpressionType.Equal, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan, ExpressionType.LessThanOrEqual };
            throw new NotSupportedException();
        }
    }
}
