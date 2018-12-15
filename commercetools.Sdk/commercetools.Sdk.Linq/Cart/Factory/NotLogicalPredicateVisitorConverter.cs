using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq.Carts
{
    public class NotLogicalPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Not;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            UnaryExpression unaryExpression = expression as UnaryExpression;
            if (unaryExpression == null)
            {
                throw new ArgumentException();
            }
            ICartPredicateVisitor cartPredicateVisitorLeft = cartPredicateVisitorFactory.Create(unaryExpression.Operand);
            return new NotLogicalPredicateVisitor(cartPredicateVisitorLeft);
        }
    }
}