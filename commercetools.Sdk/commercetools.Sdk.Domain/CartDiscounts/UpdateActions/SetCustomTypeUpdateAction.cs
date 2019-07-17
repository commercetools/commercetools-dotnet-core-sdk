namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}