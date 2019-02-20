namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class ChangeCartPredicateUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeCartPredicate";
        public string CartPredicate { get; set; }
    }
}