using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    // TODO Refactor
    public class QueryPredicateExpressionVisitor : IQueryPredicateExpressionVisitor
    {
        public static Dictionary<ExpressionType, string> MappingOfOperators = new Dictionary<ExpressionType, string>()
        {
            { ExpressionType.Equal, "=" },
            { ExpressionType.LessThan, "<" },
            { ExpressionType.GreaterThan, ">" },
            { ExpressionType.LessThanOrEqual, "<=" },
            { ExpressionType.GreaterThanOrEqual, ">=" },
            { ExpressionType.NotEqual, "!=" }
        };

        public string ProcessExpression(Expression expression)
        {
            Visitor visitor = VisitExpression(expression);
            return visitor.ToString();
        }

        public Visitor VisitExpression(Expression expression)
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
            if (MappingOfOperators.ContainsKey(expression.NodeType))
            {
                OperatorExpressionVisitor binaryExpressionVisitor = new OperatorExpressionVisitor((BinaryExpression)expression);
                return binaryExpressionVisitor;
            }
            if (expression.NodeType == ExpressionType.Call)
            {
                return Visit((MethodCallExpression)expression);
            }
            // TODO throw exception is unsupported expression
            return null;
        }

        private Visitor VisitAnd(BinaryExpression expression)
        {
            AndExpressionVisitor andExpressionVisitor = new AndExpressionVisitor(expression);
            return andExpressionVisitor;
        }

        private Visitor VisitOr(BinaryExpression expression)
        {
            OrExpressionVisitor orExpressionVisitor = new OrExpressionVisitor(expression);
            return orExpressionVisitor;
        }

        private Visitor Visit(UnaryExpression expression)
        {
            NotExpressionVisitor notExpressionVisitor = new NotExpressionVisitor(expression);
            return notExpressionVisitor;
        }

        public static List<string> GetParentMemberList(Expression expression)
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

        private Visitor Visit(MethodCallExpression expression)
        {
            if (expression.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                MethodCallExpressionVisitor methodCallExpressionVisitor = new MethodCallExpressionVisitor(expression);
                return methodCallExpressionVisitor;
            }
            if (expression.Arguments[1].NodeType == ExpressionType.Lambda)
            {
                NestedLambdaExpressionVisitor nestedLambdaExpressionVisitor = new NestedLambdaExpressionVisitor(expression);
                return nestedLambdaExpressionVisitor;
            }
            return null;
        }

        public static string Visit(string result, List<string> parentList)
        {
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
