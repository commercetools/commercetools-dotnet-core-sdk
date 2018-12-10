namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<Order>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}