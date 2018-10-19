using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class TermFacet : Facet
    {
        public Expression<Func<ProductProjection, IComparable>> Expression { get; private set; }

        public TermFacet(Expression<Func<ProductProjection, IComparable>> expression)
        {
            this.Expression = expression;
        }
    }
}
