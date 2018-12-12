using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            return ServiceLocator.Current.GetService<IQueryPredicateExpressionVisitor>()
                .ProcessExpression(this.Expression);
        }
    }
}