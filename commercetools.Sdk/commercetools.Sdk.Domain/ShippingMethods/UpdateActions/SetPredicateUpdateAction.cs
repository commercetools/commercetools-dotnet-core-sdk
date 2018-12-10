namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetPredicateUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setPredicate";
        public string Predicate { get; set; }
    }
}