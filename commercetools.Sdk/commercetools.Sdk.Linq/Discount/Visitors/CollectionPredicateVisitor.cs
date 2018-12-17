using System.Collections.Generic;

namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class CollectionPredicateVisitor : IPredicateVisitor
    {
        private readonly List<string> collection;

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