using System.Collections.Generic;

namespace commercetools.Sdk.Linq.Filter
{
    public class FilterPredicateVisitorFactory : PredicateVisitorFactoryBase
    {
        public FilterPredicateVisitorFactory(IEnumerable<IFilterPredicateVisitorConverter> registeredConverters)
            : base(registeredConverters)
        {
        }
    }
}