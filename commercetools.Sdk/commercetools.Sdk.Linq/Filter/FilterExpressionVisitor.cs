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
            // TODO Inject this instead
            FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
            FilterVisitor filterVisitor = filterVisitorFactory.Create(expression);
            return filterVisitor.Render();            
        }       
    }
}
