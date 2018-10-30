using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AccessorTraverser : IAccessorTraverser
    {
        private readonly Dictionary<string, string> mappingOfAccessors = new Dictionary<string, string>()
        {
            { "ProductId", "product.id" },
            { "CustomerId", "customer.id" }
        };

        public string Render(Expression expression)
        {
            IEnumerable<string> accessors = this.GetAccessors(expression);
            return string.Join(".", accessors.Reverse().Select(a => a.ToCamelCase()));
        }

        private IEnumerable<string> GetAccessors(Expression expression)
        {
            List<string> accessors = new List<string>();
            accessors.AddRange(ParseExpression(expression));
            return accessors;
        }

        private IEnumerable<string> ParseExpression(Expression expression)
        {
            List<string> accessors = new List<string>();
            if (expression is MemberExpression memberExpression)
            {
                string currentName = memberExpression.Member.Name;
                accessors.Add(ParseAccessorName(currentName));
                accessors.AddRange(ParseExpression(memberExpression.Expression));
            }
            if (expression is MethodCallExpression methodCallExpression)
            {
                string currentName = methodCallExpression.Method.Name;
                accessors.Add(ParseMethodAccessorName(currentName));
                accessors.AddRange(ParseExpression(methodCallExpression.Arguments[0]));
            }
            return accessors;
        }

        private string ParseAccessorName(string name)
        {
            if (this.mappingOfAccessors.ContainsKey(name))
            {
                return this.mappingOfAccessors[name];
            }
            return name;
        }

        private string ParseMethodAccessorName(string name)
        {
            if (Mappings.MethodAccessors.ContainsKey(name))
            {
                return Mappings.MethodAccessors[name];
            }
            return name;
        }
    }
}
