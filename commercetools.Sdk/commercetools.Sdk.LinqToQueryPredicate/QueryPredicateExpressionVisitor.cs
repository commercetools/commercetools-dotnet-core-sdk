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
                return VisitEqual((BinaryExpression)expression);
            }
            return null;
        }

        // TODO Refactor this
        private string VisitEqual(BinaryExpression expression)
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
            // TODO See if conversion from object to type is needed
            if (expression.Right.NodeType == ExpressionType.Constant && ((MemberExpression)expression.Left).Type == typeof(string))
            {
                right = (string)((ConstantExpression)expression.Right).Value;
            }
            
            if (parentList.Count() > 0)
            {
                string result = VisitEqual(left, right);
                foreach(string parent in parentList)
                {
                    result = $"{parent.ToCamelCase()}({result})";
                }
                return result;
            }
            return VisitEqual(left, right);
        }
        
        private string VisitEqual(string left, string right)
        {
            return $"{left.ToCamelCase()} = \"{right}\"";
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
