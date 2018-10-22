using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    // TODO Refactor
    public class FilterExpressionVisitor : IFilterExpressionVisitor
    {
        private IDictionary<string, string> extensionMethodMapping = new Dictionary<string, string>()
        {
            { "Missing", "missing" },
            { "Exists", "exists" }
        };

        // TODO Add properties if needed (for example Id in case of category) ???
        public string Render(Expression expression)
        {
            // c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e"
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return Render(((LambdaExpression)expression).Body);
            }
            // c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e"
            if (expression.NodeType == ExpressionType.Equal)
            {
                return GetEqual((BinaryExpression)expression);
            }
            // p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e")
            if (expression.NodeType == ExpressionType.Call)
            {
                return GetMethod((MethodCallExpression)expression);
            }
            // p.Categories
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return GetMember((MemberExpression)expression);
            }
            // c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id.Subtree("2fd1d652-2533-40f1-97d7-713ac24668b1")
            if (expression.NodeType == ExpressionType.Or || expression.NodeType == ExpressionType.OrElse)
            {
                return GetOr((BinaryExpression)expression);
            }
            // c
            // lambda expression parameter does not need to be returned
            if (expression.NodeType == ExpressionType.Parameter)
            {
                return string.Empty;
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string GetOr(BinaryExpression expression)
        {
            List<FilterValue> filterValues = FlattenOrFilters(expression);
            // check if all parents are the same
            if (filterValues.Any(f => f.Members.SequenceEqual(filterValues[0].Members)))
            {
                // TODO Need to group by range as well
                return RenderParents(filterValues[0].Members.Select(x => x.ToCamelCase()).ToList()) + ":" + string.Join(",", filterValues.Select(x => x.Value));
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string RenderParents(List<string> parents)
        {
            return string.Join('.', parents);
        }

        private List<FilterValue> FlattenOrFilters(BinaryExpression expression)
        {
            List<FilterValue> filterValues = new List<FilterValue>();
            Expression left = expression.Left;
            Expression right = expression.Right;
            if (left.NodeType == ExpressionType.Or || left.NodeType == ExpressionType.OrElse)
            {
                filterValues.AddRange(FlattenOrFilters((BinaryExpression)left));
            }
            else
            {
                filterValues.Add(GetFilterValue(left));
            }
            if (right.NodeType == ExpressionType.Or || right.NodeType == ExpressionType.OrElse)
            {
                filterValues.AddRange(FlattenOrFilters((BinaryExpression)right));
            }
            else
            {
                
                filterValues.Add(GetFilterValue(right));
            }
            return filterValues;
        }

        private FilterValue GetFilterValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Equal)
            {
                return new FilterValue((BinaryExpression)expression);
            }
            if (expression.NodeType == ExpressionType.Call)
            {
                return new FilterValue((MethodCallExpression)expression);
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string GetEqual(BinaryExpression expression)
        {
            string left = string.Empty;
            string right = string.Empty;
            if (expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Left).Member.Name;
            }
            if (expression.Right.NodeType == ExpressionType.Constant)
            {
                right = expression.Right.ToString();
            }
            var parent = ((MemberExpression)expression.Left).Expression;
            var parentPath = Render(parent);
            if (string.IsNullOrEmpty(parentPath))
            {
                return $"{left.ToCamelCase()}:{right}";
            }
            else
            {
                return parentPath + "." + $"{left.ToCamelCase()}:{right}";
            }
        }

        private string GetMethod(MethodCallExpression expression)
        {
            // p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e")
            if (expression.Method.Name == "Any")
            {
                return Render(expression.Arguments[0]) + "." + Render(expression.Arguments[1]);
            }
            // p => p.Categories.Missing()
            if (this.extensionMethodMapping.ContainsKey(expression.Method.Name))
            {
                return Render(expression.Arguments[0]) + ":" + this.extensionMethodMapping[expression.Method.Name];
            }
            // c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e")
            if (expression.Method.Name == "Subtree")
            {
                return Render(expression.Arguments[0]) + ":" + GetSubtree(expression.Arguments[1]);
            }
            // v => v.Price.Value.CentAmount.Range(1, 30)
            if (expression.Method.Name == "Range")
            {
                return Render(expression.Arguments[0]) + ":" + GetRange(expression.Arguments[1], expression.Arguments[2]);
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

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
            return $"range ({fromValue} to {toValue})";
        }

        private string GetSubtree(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                return " subtree(" + expression + ")";
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string GetMember(MemberExpression expression)
        {
            var parent = expression.Expression;
            var parentPath = Render(parent);
            var memberName = string.Empty;
            if (expression.Member.Name != "Value")
            {
                memberName = expression.Member.Name.ToCamelCase();
            }
            if (string.IsNullOrEmpty(parentPath))
            {
                return memberName;
            }
            else
            {
                if (memberName != string.Empty)
                { 
                    return parentPath + "." + memberName;
                }
                return parentPath;
            }
        }
    }
}
