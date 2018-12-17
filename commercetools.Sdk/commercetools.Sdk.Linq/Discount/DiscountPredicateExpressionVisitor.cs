namespace commercetools.Sdk.Linq.Discount
{
    public class DiscountPredicateExpressionVisitor : PredicateExpressionVisitorBase, IDiscountPredicateExpressionVisitor
    {
        public DiscountPredicateExpressionVisitor(DiscountPredicateVisitorFactory predicateVisitorFactory)
            : base(predicateVisitorFactory)
        {
        }
    }
}