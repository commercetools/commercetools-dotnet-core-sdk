using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class TermFacet<T> : Facet<T>
    {
        public TermFacet(Expression<Func<T, IComparable>> expression)
        {
            this.Expression = expression;
        }
    }
}
