namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeCartPredicateUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeCartPredicate";
        public string CartPredicate { get; set; }
    }
}