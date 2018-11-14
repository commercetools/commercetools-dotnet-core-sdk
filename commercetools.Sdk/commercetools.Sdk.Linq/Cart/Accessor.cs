using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class Accessor : ICartPredicateVisitor
    {
        private readonly ICartPredicateVisitor parentAccessors;
        private readonly string currentAccessor;

        public Accessor(string currentAccessor, ICartPredicateVisitor parentAccessors)
        {
            this.currentAccessor = currentAccessor;
            this.parentAccessors = parentAccessors;
        }

        public string Render()
        {
            return $"{parentAccessors.Render()}.{this.currentAccessor}";
        }
    }
}
