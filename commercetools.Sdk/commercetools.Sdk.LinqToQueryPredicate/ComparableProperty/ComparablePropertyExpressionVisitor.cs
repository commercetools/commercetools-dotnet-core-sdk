using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class ComparablePropertyExpressionVisitor : ISortExpressionVisitor, ITermFacetExpressionVisitor
    {
        public string Render(Expression expression)
        {
            // c => c.Parent
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return Render(((LambdaExpression)expression).Body);
            }
            // c.Parent.TypeId
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return GetMember((MemberExpression)expression);
            }
            // c.Slug["en"]
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

        private string GetMethod(MethodCallExpression expression)
        {
            // TODO See if a check should be added to see if the method is on a localized string only
            // in case the dictionary indexer is called, the method name is "get_Item"
            if (expression.Method.Name == "get_Item")
            {
                var key = expression.Arguments[0].ToString().Replace("\"", "");
                return Render(expression.Object) + "." + key;
            }
            if (expression.Method.Name == "FirstOrDefault")
            {
                return Render(expression.Arguments[0]);
            }
            if (expression.Method.Name == "Select")
            {
                return Render(expression.Arguments[0]) + "." + Render(expression.Arguments[1]);
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string GetMember(MemberExpression expression)
        {
            var parent = expression.Expression;
            var parentPath = Render(parent);
            if (string.IsNullOrEmpty(parentPath))
            {
                return expression.Member.Name.ToCamelCase();
            }
            else
            {
                return parentPath + "." + expression.Member.Name.ToCamelCase();
            }
        }
    }
}
