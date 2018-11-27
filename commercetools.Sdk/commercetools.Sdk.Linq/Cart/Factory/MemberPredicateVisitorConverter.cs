using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class MemberPredicateVisitorConverter : ICartPredicateVisitorConverter
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

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            if (expression is MemberExpression memberExpression)
            {
                string currentName = memberExpression.Member.Name;
                string currentAccessor = ParseName(ParseAccessorName(currentName));
                if (string.IsNullOrEmpty(currentAccessor))
                {
                    return cartPredicateVisitorFactory.Create(memberExpression.Expression);
                }
                else
                {
                    Accessor parentAccessor = cartPredicateVisitorFactory.Create(memberExpression.Expression) as Accessor;
                    ConstantPredicateVisitor constantPredicateVisitor = new ConstantPredicateVisitor(currentAccessor);
                    return new Accessor(constantPredicateVisitor, parentAccessor);
                }
            }

            throw new NotSupportedException();
        }

        private string ParseAccessorName(string name)
        {
            if (this.mappingOfAccessors.ContainsKey(name))
            {
                return this.mappingOfAccessors[name];
            }
            return name;
        }

        private string ParseName(string currentName)
        {
            if (!this.accessorsToSkip.Contains(currentName))
            {
                return currentName;
            }
            return null;
        }
    }
}