using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain.Query
{
    public class Expansion<T, S> : Expansion<T>
    {
        public Expansion(Expression<Func<T, S>> expression) : base(expression)
        {
        }
    }
}