using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class SortExpressionVisitor : ISortExpressionVisitor
    {
        public string GetPath(Expression expression)
        {
            // c => c.Parent
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return GetPath(((LambdaExpression)expression).Body);
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
                return GetPath(expression.Object) + "." + key;
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }

        private string GetMember(MemberExpression expression)
        {
            var parent = expression.Expression;
            var parentPath = GetPath(parent);
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
