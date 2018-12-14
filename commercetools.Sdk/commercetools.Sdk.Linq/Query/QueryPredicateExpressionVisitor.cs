namespace commercetools.Sdk.Linq.Query
{
    public class QueryPredicateExpressionVisitor : PredicateExpressionVisitorBase, IQueryPredicateExpressionVisitor
    {
        public QueryPredicateExpressionVisitor(QueryPredicateVisitorFactory queryPredicateVisitorFactory)
            : base(queryPredicateVisitorFactory)
        {
        }
    }
}