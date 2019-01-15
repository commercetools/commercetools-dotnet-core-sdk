namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}