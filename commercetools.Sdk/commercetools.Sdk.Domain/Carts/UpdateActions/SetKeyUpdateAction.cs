namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}