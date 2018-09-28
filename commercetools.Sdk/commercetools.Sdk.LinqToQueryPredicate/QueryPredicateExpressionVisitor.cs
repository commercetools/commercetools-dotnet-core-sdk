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
        private Dictionary<ExpressionType, string> mappingOfOperators = new Dictionary<ExpressionType, string>()
        {
            { ExpressionType.Equal, "=" },
            { ExpressionType.LessThan, "<" },
            { ExpressionType.GreaterThan, ">" },
            { ExpressionType.LessThanOrEqual, "<=" },
            { ExpressionType.GreaterThanOrEqual, ">=" }
        };

        private Dictionary<string, string> mappingOfMethods = new Dictionary<string, string>()
        {
            { "In", "in" },
            { "ContainsAll", "contains all" }
        };

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
                return Visit((BinaryExpression)expression);
            }
            if (expression.NodeType == ExpressionType.LessThan)
            {
                return Visit((BinaryExpression)expression);
            }
            if (expression.NodeType == ExpressionType.Call)
            {
                return Visit((MethodCallExpression)expression);
            }
            // TODO throw exception is unsupported expression
            return null;
        }

        // TODO Refactor this
        private string Visit(BinaryExpression expression)
        {
            string left = null;
            List<string> parentList = GetParentMemberList(expression.Left);            
            if (expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Left).Member.Name;
            }
            string right = null;
            if (expression.Right.NodeType == ExpressionType.Constant)
            {
                right = ConvertRight(expression.Right);
            }

            string operatorSign = this.mappingOfOperators[expression.NodeType];

            // TODO Add check if left or right happen to be null            
            return Visit(left, operatorSign, right, parentList);
        }

        private string ConvertRight(Expression rightExpression)
        {
            string right = null;
            Type typeOfRight = ((ConstantExpression)rightExpression).Value.GetType();
            object result = ((ConstantExpression)rightExpression).Value;
            if (typeOfRight == typeof(string))
            {
                right = $"\"{result.ToString()}\"";
            }
            else
            {
                right = $"{result.ToString()}";
            }
            return right;
        }

        private List<string> GetParentMemberList(Expression expression)
        {
            List<string> parentList = new List<string>();
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var parent = ((MemberExpression)expression).Expression;
                while (parent is MemberExpression)
                {
                    parentList.Add(((MemberExpression)parent).Member.Name);
                    parent = ((MemberExpression)parent).Expression;
                }
            }
            return parentList;
        }

        private string Visit(MethodCallExpression expression)
        {
            string left = null;
            string right = null;
            string operatorSign = this.mappingOfMethods[expression.Method.Name];

            List<string> parentList = GetParentMemberList(expression.Arguments[0]);
            if (expression.Arguments[0].NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Arguments[0]).Member.Name;
            }

            List<string> rightList = new List<string>();
            if (expression.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                foreach(Expression part in ((NewArrayExpression)expression.Arguments[1]).Expressions)
                {
                    if (part.NodeType == ExpressionType.Constant)
                    {
                        rightList.Add(ConvertRight(part));
                    }
                }
            }

            if (rightList.Count() > 0)
            { 
                right = $"({string.Join(", ", rightList)})";
            }

            return Visit(left, operatorSign, right, parentList);
        }

        private string Visit(string left, string operatorSign, string right, List<string> parentList)
        {
            string result = $"{left.ToCamelCase()} {operatorSign.ToCamelCase()} {right}";
            if (parentList.Count() > 0)
            {
                foreach (string parent in parentList)
                {
                    result = $"{parent.ToCamelCase()}({result})";
                }
                return result;
            }
            return result;
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
