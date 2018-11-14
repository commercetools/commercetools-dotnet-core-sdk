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
            { "CustomerId", "customer.id" },
            { "TotalNet", "net" },
            { "TotalGross", "gross" }
        };

        private readonly IEnumerable<string> accessorsToSkip = new List<string>()
        {
            "Value", 
            "Key"
        };

        public List<string> GetAccessorsForExpression(Expression expression)
        {
            List<string> accessors = new List<string>();
            accessors.AddRange(ParseExpression(expression));
            return accessors;
        } 

        public List<string> GetAccessorsForExpression(Expression expression, List<string> additions)
        {
            List<string> accessors = new List<string>();
            accessors.AddRange(additions);
            accessors.AddRange(GetAccessorsForExpression(expression));
            return accessors;
        }

        private List<string> ParseExpression(Expression expression)
        {
            List<string> accessors = new List<string>();
            if (expression is MemberExpression memberExpression)
            {
                string currentName = memberExpression.Member.Name;
                accessors.AddRange(AddName(ParseAccessorName(currentName)));
                accessors.AddRange(ParseExpression(memberExpression.Expression));
            }
            if (expression is MethodCallExpression methodCallExpression)
            {
                string currentName = methodCallExpression.Method.Name;
                accessors.AddRange(AddName(ParseMethodAccessorName(currentName)));
                accessors.AddRange(ParseExpression(methodCallExpression.Arguments[0]));
            }
            return accessors;
        }

        private List<string> AddName(string currentName)
        {
            List<string> names = new List<string>();
            if (!this.accessorsToSkip.Contains(currentName))
            {
                names.Add(currentName);
            }
            return names;
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
