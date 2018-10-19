using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class FilterFacet : Facet
    {
        public Expression<Func<ProductProjection, bool>> Expression { get; private set; }

        public FilterFacet(Expression<Func<ProductProjection, bool>> expression)
        {
            this.Expression = expression;
        }
    }
}
