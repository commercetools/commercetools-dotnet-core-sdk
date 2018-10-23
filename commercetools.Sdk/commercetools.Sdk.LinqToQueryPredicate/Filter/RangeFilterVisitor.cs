using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class RangeFilterVisitor
    {
        public string From { get; private set; }
        public string To { get; private set; }        

        public RangeFilterVisitor(MethodCallExpression expression)
        {
            this.From = GetRange(expression.Arguments[1]);
            this.To = GetRange(expression.Arguments[2]);
        }

        private string GetRange(Expression expression)
        {
            if (expression == null)
            { 
                throw new ArgumentNullException("The expression does not have all expected properties set.");
            }
            if (expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value == null)
            {
                return "*";
            }
            if (expression.NodeType == ExpressionType.Convert && ((UnaryExpression)expression).Operand.NodeType == ExpressionType.Constant)
            {
                return ((UnaryExpression)expression).Operand.ToString();
            }
 
            throw new NotSupportedException("The expression type is not supported.");           
        }

        public string Render()
        {
            return $"({this.From} to {this.To})";
        }
    }
}
