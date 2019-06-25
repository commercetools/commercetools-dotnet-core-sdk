namespace commercetools.Sdk.Linq.Sort
{
    public class SortPredicateExpressionVisitor : PredicateExpressionVisitorBase, ISortPredicateExpressionVisitor
    {
        public SortPredicateExpressionVisitor()
            : base(new SortPredicateVisitorFactory())
        {
        }
    }
}
