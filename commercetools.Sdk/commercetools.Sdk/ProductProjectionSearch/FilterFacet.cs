using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class RangeFacet : Facet
    {
        public Expression<Func<ProductProjection, bool>> Expression { get; private set; }

        public RangeFacet(Expression<Func<ProductProjection, bool>> expression)
        {
            this.Expression = expression;
        }
    }
}
