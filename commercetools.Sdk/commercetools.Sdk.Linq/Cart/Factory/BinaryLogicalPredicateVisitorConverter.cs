using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Carts;

namespace commercetools.Sdk.Linq
{
    public class BinaryLogicalPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly Dictionary<ExpressionType, string> mappingOfOperators = new Dictionary<ExpressionType, string>()
        {
            { ExpressionType.And, LogicalOperators.AND },
            { ExpressionType.AndAlso, LogicalOperators.AND },
            { ExpressionType.Or, LogicalOperators.OR },
            { ExpressionType.OrElse, LogicalOperators.OR }
        };

        public bool CanConvert(Expression expression)
        {
            return this.mappingOfOperators.ContainsKey(expression.NodeType);
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                throw new ArgumentException();
            }
            ICartPredicateVisitor cartPredicateVisitorLeft = cartPredicateVisitorFactory.Create(binaryExpression.Left);
            ICartPredicateVisitor cartPredicateVisitorRight = cartPredicateVisitorFactory.Create(binaryExpression.Right);
            string operatorSign = GetOperator(expression.NodeType);
            return new BinaryLogicalPredicateVisitor(cartPredicateVisitorLeft, operatorSign, cartPredicateVisitorRight);
        }

        private string GetOperator(ExpressionType expressionType)
        {
            if (this.mappingOfOperators.ContainsKey(expressionType))
            {
                return this.mappingOfOperators[expressionType];
            }
            throw new NotSupportedException();
        }
    }
}