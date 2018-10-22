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

        private List<ExpressionType> allowedGroupingExpressionTypes = new List<ExpressionType>() { ExpressionType.Or, ExpressionType.OrElse, ExpressionType.And, ExpressionType.AndAlso };

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
            if (allowedGroupingExpressionTypes.Contains(expression.NodeType))
            {
                return GetGroup((BinaryExpression)expression);
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

        private string GetGroup(BinaryExpression expression)
        {
            List<FilterVisitor> filterValues = GroupExpressionFlattener.GetFilterVisitors(expression);
            // check if all parents are the same, otherwise grouping is not possible
            if (filterValues.Any(f => f.Accessors.SequenceEqual(filterValues[0].Accessors)))
            {
                // TODO See how to treat else case
                if (filterValues.All(f => f is RangeFilterVisitor))
                {
                    return RenderParents(filterValues[0].Accessors.Select(x => x.ToCamelCase()).ToList()) + ":range " + string.Join(", ", filterValues.Select(x => x.Render()));
                }
                return RenderParents(filterValues[0].Accessors.Select(x => x.ToCamelCase()).ToList()) + ": " + string.Join(", ", filterValues.Select(x => x.Render()));
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string RenderParents(List<string> parents)
        {
            return string.Join('.', parents);
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
                SubtreeFilterVisitor subtreeFilterVisitor = new SubtreeFilterVisitor(expression);
                return AccessorTraverser.RenderAccessors(subtreeFilterVisitor.Accessors) + ": " + subtreeFilterVisitor.Render();
                //return Render(expression.Arguments[0]) + ":" + GetSubtree(expression.Arguments[1]);
            }
            // v => v.Price.Value.CentAmount.Range(1, 30)
            if (expression.Method.Name == "Range")
            {
                RangeFilterVisitor rangeFilterVisitor = new RangeFilterVisitor(expression);
                return AccessorTraverser.RenderAccessors(rangeFilterVisitor.Accessors) + ":" + "range " + rangeFilterVisitor.Render();
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
