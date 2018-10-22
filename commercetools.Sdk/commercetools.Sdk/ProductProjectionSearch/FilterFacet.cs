using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class FilterFacet<T> : Facet<T>
    {
        public Expression<Func<T, bool>> Expression { get; private set; }

        public FilterFacet(Expression<Func<T, bool>> expression)
        {
            this.Expression = expression;
        }
    }
}
