using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class FilterValue
    {
        public List<string> Members { get; private set; } = new List<string>();
        public string Value { get; private set; }

        public FilterValue(BinaryExpression expression)
        {            
            if (expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                this.Members.Add(((MemberExpression)expression.Left).Member.Name);
            }
            this.Members.AddRange(GetParentMembers(expression));
            if (expression.Right.NodeType == ExpressionType.Constant)
            {
                this.Value = " " + expression.Right.ToString();
            }
        }

        public FilterValue(MethodCallExpression expression)
        {
            if (expression.Arguments[0].NodeType == ExpressionType.MemberAccess)
            {
                this.Members.Add(((MemberExpression)expression.Arguments[0]).Member.Name);
            }
            this.Members.AddRange(GetParentMembers(expression));
            if (expression.Method.Name == "Subtree")
            {
                this.Value = GetSubtree(expression.Arguments[1]);
            }
            if (expression.Method.Name == "Range")
            {
                this.Value = GetRange(expression.Arguments[1], expression.Arguments[2]);
            }
        }

        // TODO will not be doubled after refactoring
        private string GetSubtree(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                return " subtree(" + expression + ")";
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        // TODO will not be doubled after refactoring
        private string GetRange(Expression from, Expression to)
        {
            string fromValue = string.Empty;
            string toValue = string.Empty;
            if (from.NodeType == ExpressionType.Constant && ((ConstantExpression)from).Value == null)
            {
                fromValue = "*";
            }
            else if (from.NodeType == ExpressionType.Convert && ((UnaryExpression)from).Operand.NodeType == ExpressionType.Constant)
            {
                fromValue = ((UnaryExpression)from).Operand.ToString();
            }
            else
            {
                // TODO Move message to a resource file
                throw new NotSupportedException("The expression type is not supported.");
            }
            if (to.NodeType == ExpressionType.Constant && ((ConstantExpression)to).Value == null)
            {
                toValue = "*";
            }
            else if (to.NodeType == ExpressionType.Convert && ((UnaryExpression)to).Operand.NodeType == ExpressionType.Constant)
            {
                toValue = ((UnaryExpression)to).Operand.ToString();
            }
            else
            {
                // TODO Move message to a resource file
                throw new NotSupportedException("The expression type is not supported.");
            }
            return $" ({fromValue} to {toValue})";
        }

        private static List<string> GetParentMembers(Expression expression)
        {
            List<string> members = new List<string>();
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var parent = ((MemberExpression)expression).Expression;
                while (parent is MemberExpression)
                {
                    members.Add(((MemberExpression)parent).Member.Name);
                    parent = ((MemberExpression)parent).Expression;
                }
            }
            return members;
        }
    }
}
