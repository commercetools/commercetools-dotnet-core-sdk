namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class MethodPredicateVisitor : IPredicateVisitor
    {
        private readonly string method;
        private readonly IPredicateVisitor innerPredicate;

        public MethodPredicateVisitor(string method, IPredicateVisitor innerPredicate)
        {
            this.method = method;
            this.innerPredicate = innerPredicate;
        }

        public string Render()
        {
            return $"{this.method.ToCamelCase()}({this.innerPredicate.Render()})";
        }
    }
}