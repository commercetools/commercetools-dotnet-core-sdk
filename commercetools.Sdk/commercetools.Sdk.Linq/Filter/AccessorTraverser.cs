using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AccessorTraverser
    {
        private static List<string> namesToSkip = new List<string>() { "value" };

        public static List<string> GetAccessors(Expression expression) 
        {
            List<string> accessors = new List<string>();
            ParseName(accessors, expression);
            accessors.Reverse();
            return accessors;
        }

        private static void ParseName(List<string> accessors, Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                string currentName = memberExpression.Member.Name.ToCamelCase();
                AddName(accessors, currentName);
                ParseName(accessors, memberExpression.Expression);
            }
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "get_Item")
                {
                    var index = methodCallExpression.Arguments[0].ToString().Replace("\"", "");
                    AddName(accessors, index);
                    ParseName(accessors, methodCallExpression.Object);
                }
                Expression accessorExpression = methodCallExpression.Arguments[0];
                if (accessorExpression is MemberExpression attributeMemberExpression && attributeMemberExpression.Member.Name == "Attributes")
                {
                    AttributeFilterVisitor attributeFilterVisitor = new AttributeFilterVisitor(methodCallExpression);
                    // TODO Fix this in another way, this needs to be reversed back since the main list will be reversed after
                    attributeFilterVisitor.Accessors.Reverse();
                    accessors.AddRange(attributeFilterVisitor.Accessors);
                }
            }
        }

        private static void AddName(List<string> accessors, string name)
        {
            if (!namesToSkip.Contains(name))
            {
                accessors.Add(name);
            }
        }

        public static string RenderAccessors(List<string> accessors)
        {
            return string.Join(".", accessors);
        }
    }
}
