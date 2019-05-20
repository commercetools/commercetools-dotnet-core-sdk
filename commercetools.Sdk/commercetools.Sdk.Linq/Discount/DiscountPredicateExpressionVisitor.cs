namespace commercetools.Sdk.Linq.Discount
{
    public class DiscountPredicateExpressionVisitor : PredicateExpressionVisitorBase, IDiscountPredicateExpressionVisitor
    {
        public DiscountPredicateExpressionVisitor()
            : base(new DiscountPredicateVisitorFactory())
        {
        }
    }
}
