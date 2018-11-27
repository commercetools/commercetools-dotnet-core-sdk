using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
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