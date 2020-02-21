namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}