using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain.ProductProjections
{
    public class Filter<T>
    {
        public Expression<Func<T, bool>> Expression { get; private set; }

        public Filter(Expression<Func<T, bool>> expression)
        {
            this.Expression = expression;
        }

        public override string ToString()
        {
            return ServiceLocator.Current.GetService<IFilterExpressionVisitor>().Render(this.Expression);
        }
    }
}