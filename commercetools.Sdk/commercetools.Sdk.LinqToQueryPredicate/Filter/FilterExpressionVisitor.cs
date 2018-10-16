using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class FilterExpressionVisitor : IFilterExpressionVisitor
    {
        // TODO Add properties if needed (for example Id in case of category)
        public string Render(Expression expression)
        {
            return null;
        }
    }
}
