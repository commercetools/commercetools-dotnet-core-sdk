using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public abstract class GroupFilterVisitor<T> : GroupFilterVisitor
    {
        protected List<T> innerFilters = new List<T>();
    }
}
