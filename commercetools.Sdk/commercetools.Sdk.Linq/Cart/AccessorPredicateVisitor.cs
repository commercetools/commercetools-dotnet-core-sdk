using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AccessorPredicateVisitor : ICartPredicateVisitor
    {
        private readonly IEnumerable<string> accessors;

        public AccessorPredicateVisitor(IEnumerable<string> accessors)
        {
            this.accessors = accessors;
        }

        public string Render()
        {
            return string.Join(".", this.accessors.Reverse().Select(a => a.ToCamelCase()));
        }
    }
}
