using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class ComparisonPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            return Mapping.ComparisonOperators.ContainsKey(expression.NodeType);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            IPredicateVisitor left = predicateVisitorFactory.Create(binaryExpression.Left);
            string operatorSign = Mapping.GetOperator(expression.NodeType, Mapping.ComparisonOperators);
            IPredicateVisitor right = predicateVisitorFactory.Create(binaryExpression.Right);
            ComparisonPredicateVisitor simplePredicateVisitor = new ComparisonPredicateVisitor(left, operatorSign, right);
            return simplePredicateVisitor;
        }
    }
}