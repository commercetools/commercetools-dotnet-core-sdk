using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class QueryPredicateExpressionVisitor
    {
        public string ProcessExpression(Expression expression)
        {
            return VisitExpression(expression);
        }

        private string VisitExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return VisitExpression(((LambdaExpression)expression).Body);
            }
            if (expression.NodeType == ExpressionType.Equal)
            {
                return Visit((BinaryExpression)expression, "=");
            }
            if (expression.NodeType == ExpressionType.LessThan)
            {
                return Visit((BinaryExpression)expression, "<");
            }
            return null;
        }

        // TODO Refactor this
        private string Visit(BinaryExpression expression, string operatorSign)
        {
            string left = null;
            List<string> parentList = new List<string>();
            if (expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                var parent = ((MemberExpression)expression.Left).Expression;
                while (parent is MemberExpression)
                {
                    parentList.Add(((MemberExpression)parent).Member.Name);
                    parent = ((MemberExpression)parent).Expression;
                }
                left = ((MemberExpression)expression.Left).Member.Name;
            }
            string right = null;
            if (expression.Right.NodeType == ExpressionType.Constant)
            {
                Type typeOfLeft = ((MemberExpression)expression.Left).Type;
                object result = Convert.ChangeType(((ConstantExpression)expression.Right).Value, typeOfLeft);
                if (typeOfLeft == typeof(string))
                {
                    right = $"\"{result.ToString()}\"";
                }
                else
                {
                    right = $"{result.ToString()}";
                }
            }
            
            if (parentList.Count() > 0)
            {
                string result = Visit(left, operatorSign, right);
                foreach(string parent in parentList)
                {
                    result = $"{parent.ToCamelCase()}({result})";
                }
                return result;
            }
            return Visit(left, operatorSign, right);
        }

        private string Visit(string left, string operatorSign, string right)
        {
            return $"{left.ToCamelCase()} {operatorSign} {right}";
        }
    }

    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}
