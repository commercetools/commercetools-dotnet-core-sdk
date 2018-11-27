using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class ComparisonPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly Dictionary<ExpressionType, string> mappingOfOperators = new Dictionary<ExpressionType, string>()
        {
            { ExpressionType.Equal, "=" },
            { ExpressionType.LessThan, "<" },
            { ExpressionType.GreaterThan, ">" },
            { ExpressionType.LessThanOrEqual, "<=" },
            { ExpressionType.GreaterThanOrEqual, ">=" },
            { ExpressionType.NotEqual, "!=" }
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
            ICartPredicateVisitor left = cartPredicateVisitorFactory.Create(binaryExpression.Left);
            string operatorSign = GetOperator(expression.NodeType);
            ICartPredicateVisitor right = cartPredicateVisitorFactory.Create(binaryExpression.Right);
            ComparisonPredicateVisitor simplePredicateVisitor = new ComparisonPredicateVisitor(left, operatorSign, right);
            return simplePredicateVisitor;
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