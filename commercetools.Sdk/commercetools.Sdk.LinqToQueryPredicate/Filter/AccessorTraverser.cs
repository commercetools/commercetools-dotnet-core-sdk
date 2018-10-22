using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class AccessorTraverser
    {
        private static List<string> namesToSkip = new List<string>() { "value" };

        public static List<string> GetAccessors(Expression expression) 
        {
            List<string> accessors = new List<string>();
            if (expression is MemberExpression memberExpression)
            {
                string currentName = memberExpression.Member.Name.ToCamelCase();
                accessors.Add(currentName);
                var parentAccessor = memberExpression.Expression;
                while (parentAccessor is MemberExpression parentMemberExpression)
                {
                    string parentName = parentMemberExpression.Member.Name.ToCamelCase();
                    if (!namesToSkip.Contains(parentName))
                    { 
                        accessors.Add(parentName);
                    }
                    parentAccessor = parentMemberExpression.Expression;
                }
            }
            accessors.Reverse();
            return accessors;
        }

        public static string RenderAccessors(List<string> accessors)
        {
            return string.Join('.', accessors);
        }
    }
}
