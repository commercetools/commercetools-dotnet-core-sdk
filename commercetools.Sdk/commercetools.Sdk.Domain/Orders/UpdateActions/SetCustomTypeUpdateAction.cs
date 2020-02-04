namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}