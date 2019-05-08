using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Linq.Query
{
    public class QueryPredicateVisitorFactory : PredicateVisitorFactoryBase
    {
        public QueryPredicateVisitorFactory()
            : base(GetPredicateVisitorConverters<IQueryPredicateVisitorConverter>())
        {
        }
    }
}
