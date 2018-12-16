using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class EqualPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Equal;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            IPredicateVisitor left = predicateVisitorFactory.Create(binaryExpression.Left);
            IPredicateVisitor right = predicateVisitorFactory.Create(binaryExpression.Right);
            return new EqualPredicateVisitor(left, right);
        }
    }
}
