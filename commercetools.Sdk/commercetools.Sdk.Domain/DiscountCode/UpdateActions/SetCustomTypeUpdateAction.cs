namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetCustomTypeUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}