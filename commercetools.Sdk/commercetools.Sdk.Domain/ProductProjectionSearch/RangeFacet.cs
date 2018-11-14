using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
