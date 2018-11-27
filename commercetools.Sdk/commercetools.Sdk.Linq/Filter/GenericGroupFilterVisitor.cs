using System.Collections.Generic;

namespace commercetools.Sdk.Linq
{
    public abstract class GroupFilterVisitor<T> : GroupFilterVisitor
    {
        protected List<T> innerFilters = new List<T>();
    }
}