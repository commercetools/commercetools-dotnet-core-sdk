using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Linq.Query;

namespace commercetools.Sdk.Domain.Query
{
    public class QueryPredicate<T>
    {
        public Expression<Func<T, bool>> Expression { get; set; }

        public QueryPredicate(Expression<Func<T, bool>> expression)
        {
            this.Expression = expression;
        }

        public override string ToString()
        {
            return this.Expression.RenderQueryPredicate();
        }
    }
}
