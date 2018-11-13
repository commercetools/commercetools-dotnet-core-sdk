using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AttributePredicateVisitor : ICartPredicateVisitor
    {
        private IEnumerable<string> accessors;
        private ICartPredicateVisitor attributeValuePredicate;

        public AttributePredicateVisitor(IEnumerable<string> accessors, ICartPredicateVisitor attributeValuePredicate)
        {
            this.accessors = accessors;
            this.attributeValuePredicate = attributeValuePredicate;
        }

        public string Render()
        {
            string result = $"{string.Join(".", this.accessors.Reverse().Select(a => a.ToCamelCase()))}.{this.attributeValuePredicate.Render()}";
            // it can happen that value predicate does not have any accessor (in case of value or key)
            return result.Replace(". ", " ");
        }
    }
}
