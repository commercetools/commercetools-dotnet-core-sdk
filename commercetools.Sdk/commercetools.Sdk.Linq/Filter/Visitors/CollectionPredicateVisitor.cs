using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class CollectionPredicateVisitor : IPredicateVisitor
    {
        private readonly IEnumerable<IPredicateVisitor> predicateVisitors;

        public CollectionPredicateVisitor(IEnumerable<IPredicateVisitor> predicateVisitors)
        {
            this.predicateVisitors = predicateVisitors;
        }

        public string Render()
        {
            return $"{string.Join(",", this.predicateVisitors.Select(p => p.Render()))}".Replace("  ", " ");
        }
    }
}
