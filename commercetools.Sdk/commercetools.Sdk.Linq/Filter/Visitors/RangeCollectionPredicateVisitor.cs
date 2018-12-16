using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class RangeCollectionPredicateVisitor : IPredicateVisitor
    {
        public IEnumerable<RangePredicateVisitor> Ranges { get; }

        public RangeCollectionPredicateVisitor(IEnumerable<RangePredicateVisitor> ranges)
        {
            this.Ranges = ranges;
        }

        public string Render()
        {
            return $"range {string.Join(", ", this.Ranges.Select(r => r.Render()))}";
        }
    }
}
