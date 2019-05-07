namespace commercetools.Sdk.Linq.Filter
{
    public class FilterPredicateExpressionVisitor : PredicateExpressionVisitorBase, IFilterPredicateExpressionVisitor
    {
        public FilterPredicateExpressionVisitor()
            : base(new FilterPredicateVisitorFactory())
        {
        }
    }
}
