using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class CollectionPredicateVisitor : IPredicateVisitor
    {
        private readonly IEnumerable<IPredicateVisitor> collection;

        public CollectionPredicateVisitor(IEnumerable<IPredicateVisitor> collection)
        {
            this.collection = collection;
        }

        public string Render()
        {
            return $"({string.Join(", ", this.collection.Select(p => p.Render()))})";
        }
    }
}