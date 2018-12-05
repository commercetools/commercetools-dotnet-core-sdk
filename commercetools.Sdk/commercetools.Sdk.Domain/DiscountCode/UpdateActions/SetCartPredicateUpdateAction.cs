namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetCartPredicateUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setCartPredicate";
        public string CartPredicate { get; set; }
    }
}