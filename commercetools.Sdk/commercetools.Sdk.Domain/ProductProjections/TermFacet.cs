﻿using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain.ProductProjections
{
    public class TermFacet<T> : Facet<T>
    {
        public TermFacet(Expression<Func<T, IComparable>> expression)
        {
            this.Expression = expression;
        }
    }
}