using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class GroupExpressionParser
    {
        private List<ExpressionType> allowedGroupExpressionTypes = new List<ExpressionType>() { ExpressionType.Or, ExpressionType.OrElse, ExpressionType.And, ExpressionType.AndAlso };

        public List<Expression> FlattenExpressions(BinaryExpression expression)
        {
            List<Expression> expressions = new List<Expression>();
            expressions.AddRange(ParseSide(expression.Left));
            expressions.AddRange(ParseSide(expression.Right));
            return expressions;
        }

        private List<Expression> ParseSide(Expression expression)
        {
            List<Expression> expressions = new List<Expression>();
            if (allowedGroupExpressionTypes.Contains(expression.NodeType))
            {
                expressions.AddRange(FlattenExpressions((BinaryExpression)expression));
            }
            else
            {
               expressions.Add(expression);
            }
            return expressions;
        }
    }
}
