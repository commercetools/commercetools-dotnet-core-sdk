using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class NotLogicalPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Not;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            UnaryExpression unaryExpression = expression as UnaryExpression;
            if (unaryExpression == null)
            {
                return null;
            }

            IPredicateVisitor cartPredicateVisitorLeft = predicateVisitorFactory.Create(unaryExpression.Operand);
            return new NotLogicalPredicateVisitor(cartPredicateVisitorLeft);
        }
    }
}