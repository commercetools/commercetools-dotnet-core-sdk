using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    public class RangeFacet<T> : Facet<T>
    {
        public RangeFacet(Expression<Func<T, bool>> expression)
        {
            this.Expression = expression;
        }
    }
}