namespace commercetools.Sdk.Linq.Query
{
    public class QueryPredicateExpressionVisitor : PredicateExpressionVisitorBase, IQueryPredicateExpressionVisitor
    {
        public QueryPredicateExpressionVisitor()
            : base(new QueryPredicateVisitorFactory())
        {
        }
    }
}
