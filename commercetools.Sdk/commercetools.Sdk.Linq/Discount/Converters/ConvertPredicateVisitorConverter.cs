using System.Linq.Expressions;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class ConvertPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                return true;
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            UnaryExpression unaryExpression = expression as UnaryExpression;
            if (unaryExpression == null)
            {
                return null;
            }

            return predicateVisitorFactory.Create(unaryExpression.Operand);
        }
    }
}
