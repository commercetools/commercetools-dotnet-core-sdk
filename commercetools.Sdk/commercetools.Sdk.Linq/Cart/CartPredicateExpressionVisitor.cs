namespace commercetools.Sdk.Linq
{
    using System.Linq.Expressions;

    public class CartPredicateExpressionVisitor : ICartPredicateExpressionVisitor
    {
        private readonly ICartPredicateVisitorFactory cartPredicateVisitorFactory;

        public CartPredicateExpressionVisitor(ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            this.cartPredicateVisitorFactory = cartPredicateVisitorFactory;
        }

        public string Render(Expression expression)
        {
            ICartPredicateVisitor cartPredicateVisitor = this.cartPredicateVisitorFactory.Create(expression);
            return cartPredicateVisitor.Render();
        }
    }
}