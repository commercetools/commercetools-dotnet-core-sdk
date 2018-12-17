namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class NotLogicalPredicateVisitor : IPredicateVisitor
    {
        private readonly IPredicateVisitor innerPredicate;

        public NotLogicalPredicateVisitor(IPredicateVisitor innerPredicate)
        {
            this.innerPredicate = innerPredicate;
        }

        public string Render()
        {
            return $"{Mapping.Not}({this.innerPredicate.Render()})";
        }
    }
}