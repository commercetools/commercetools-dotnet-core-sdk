using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
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
            // Range(null, 30); null is a constant
            if (expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value == null)
            {
                return "*";
            }
            // Range(1, 30); Range extension method accepts Nullable<T> so that conversion takes place under the hood; Convert(1, Nullable`1)
            if (expression.NodeType == ExpressionType.Convert && ((UnaryExpression)expression).Operand.NodeType == ExpressionType.Constant)
            {
                return ((UnaryExpression)expression).Operand.ToString();
            }
            // DateTime.Parse("2015-06-04T12:27:55.344Z")
            if (expression.NodeType == ExpressionType.Convert && ((UnaryExpression)expression).Operand.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = ((UnaryExpression)expression).Operand as MethodCallExpression;
                return methodCallExpression.Arguments[0].ToString();
            }
            // Value.Range(36, 42); Value is double and parameter is int so that conversion takes places under the hood
            if (expression.NodeType == ExpressionType.Convert && ((UnaryExpression)expression).Operand.NodeType == ExpressionType.Convert)
            {
                UnaryExpression unaryExpression = ((UnaryExpression)expression).Operand as UnaryExpression;
                return unaryExpression.Operand.ToString();
            }

            throw new NotSupportedException("The expression type is not supported.");
        }

        public string Render()
        {
            return $"({this.From} to {this.To})";
        }
    }
}