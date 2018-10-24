using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class FilterExpressionVisitor : IFilterExpressionVisitor
    {
        public string Render(Expression expression)
        {
            FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
            return filterVisitorFactory.CreateFilterVisitor(expression).Render();            
        }       
    }
}
