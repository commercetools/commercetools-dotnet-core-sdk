using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain.Query
{
    public class ReferenceExpansion<T> : Expansion<T, Reference>
    {
        public ReferenceExpansion(Expression<Func<T, Reference>> expression) : base(expression)
        {
        }
    }
}
