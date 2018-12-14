using System.Collections.Generic;

namespace commercetools.Sdk.Linq.Query
{
    public class QueryPredicateVisitorFactory : PredicateVisitorFactoryBase
    {
        public QueryPredicateVisitorFactory(IEnumerable<IQueryPredicateVisitorConverter> registeredConverters)
            : base(registeredConverters)
        {
        }
    }
}