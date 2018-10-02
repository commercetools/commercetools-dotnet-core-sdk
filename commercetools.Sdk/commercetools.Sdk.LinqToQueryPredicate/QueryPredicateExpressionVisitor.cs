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
            { ExpressionType.GreaterThanOrEqual, ">=" },
            { ExpressionType.NotEqual, "!=" }
        };

        private Dictionary<string, string> mappingOfMethods = new Dictionary<string, string>()
        {
            { "In", "in" },
            { "NotIn", "not in" },
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
            if (expression.NodeType == ExpressionType.Not)
            {
                return Visit((UnaryExpression)expression);
            }
            if (expression.NodeType == ExpressionType.And || expression.NodeType == ExpressionType.AndAlso)
            {
                return VisitAnd((BinaryExpression)expression);
            }
            if (expression.NodeType == ExpressionType.Or || expression.NodeType == ExpressionType.OrElse)
            {
                return VisitOr((BinaryExpression)expression);
            }
            if (this.mappingOfOperators.ContainsKey(expression.NodeType))
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

        private string VisitAnd(BinaryExpression expression)
        {
            return $"{VisitExpression(expression.Left)} and {VisitExpression(expression.Right)}";
        }

        private string VisitOr(BinaryExpression expression)
        {
            return $"{VisitExpression(expression.Left)} or {VisitExpression(expression.Right)}";
        }

        private string Visit(UnaryExpression expression)
        {
            return $"not({VisitExpression(expression.Operand)})";
        }

        // TODO Refactor this
        private string Visit(BinaryExpression expression)
        {
            string left = null;
            List<string> parentList = new List<string>();
            if (expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Left).Member.Name;
                parentList = GetParentMemberList(expression.Left);
            }
            if (expression.Left.NodeType == ExpressionType.Call)
            {
                // TODO See if a check should be made to see if this is a dictionary<string, string>
                object name = ((MethodCallExpression)expression.Left).Object;
                if (((MethodCallExpression)expression.Left).Object.NodeType == ExpressionType.MemberAccess)
                {
                    parentList.Add(((MemberExpression)((MethodCallExpression)expression.Left).Object).Member.Name);
                    parentList.AddRange(GetParentMemberList(((MethodCallExpression)expression.Left).Object));                    
                }
                if (((MethodCallExpression)expression.Left).Arguments[0].NodeType == ExpressionType.Constant)
                {
                    left = ((MethodCallExpression)expression.Left).Arguments[0].ToString().Replace("\"", ""); 
                }
            }
            string right = null;
            if (expression.Right.NodeType == ExpressionType.Constant)
            {
                right = expression.Right.ToString();
            }

            string operatorSign = this.mappingOfOperators[expression.NodeType];

            // TODO Add check if left or right happen to be null            
            return Visit(left, operatorSign, right, parentList);
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
                        rightList.Add(part.ToString());
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
