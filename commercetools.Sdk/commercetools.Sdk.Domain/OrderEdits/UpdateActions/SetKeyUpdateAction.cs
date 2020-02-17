namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}