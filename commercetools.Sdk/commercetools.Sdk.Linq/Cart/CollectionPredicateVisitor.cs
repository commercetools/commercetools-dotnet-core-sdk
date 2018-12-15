using System.Collections.Generic;

namespace commercetools.Sdk.Linq.Carts
{
    public class CollectionPredicateVisitor : ICartPredicateVisitor
    {
        private List<string> collection;

        public CollectionPredicateVisitor(List<string> collection)
        {
            this.collection = collection;
        }

        public string Render()
        {
            return $"({string.Join(", ", this.collection)})";
        }
    }
}