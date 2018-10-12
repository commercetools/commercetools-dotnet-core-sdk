using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class ExpansionVisitor : IExpansionVisitor
    {
        private IDictionary<string, string> expandMethodMapping = new Dictionary<string, string>()
        {
            { "ExpandAll", "[*]" },
            { "ExpandProductSlugs", "[*].productSlug" },
            { "ExpandVariants", "[*].variant" },
            { "ExpandValues", "[*].value" }
        };

        public string GetPath(Expression expression)
        {
            // c => c.Parent
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return GetPath(((LambdaExpression)expression).Body);
            }
            // c.Parent
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return GetMember((MemberExpression)expression);
            }
            // c.Ancestors[0] or c.Ancestors.ExpandAll()
            if (expression.NodeType == ExpressionType.Call)
            {
                return GetMethod((MethodCallExpression)expression);
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

        private string GetMember(MemberExpression expression)
        {
            var parent = expression.Expression;
            var parentPath = GetPath(parent);
            var memberName = string.Empty;
            // c.Parent.Obj.Parent
            // Obj does not need to be returned
            if (expression.Member.Name == "Obj")
            {
                // Obj always has a parent path
                return parentPath + string.Empty;
            }
            if (string.IsNullOrEmpty(parentPath))
            {
                return expression.Member.Name.ToCamelCase();
            }
            else
            {
                return parentPath + "." + expression.Member.Name.ToCamelCase();
            }
        }

        private string GetMethod(MethodCallExpression expression)
        {
            // c.Ancestors[0]
            // in case the list indexer is called, the method name is "get_Item"
            if (expression.Method.Name == "get_Item")
            {
                var index = expression.Arguments[0].ToString();
                return GetPath(expression.Object) + $"[{index}]";
            }
            // c.Ancestors.ExpandAll()
            if (this.expandMethodMapping.ContainsKey(expression.Method.Name))
            {
                return GetPath(expression.Arguments[0]) + this.expandMethodMapping[expression.Method.Name];
            }
            // TODO Move to resource file and also add the name of the method
            throw new NotSupportedException("The method call is not supported in this expression.");
        }
    }
}
