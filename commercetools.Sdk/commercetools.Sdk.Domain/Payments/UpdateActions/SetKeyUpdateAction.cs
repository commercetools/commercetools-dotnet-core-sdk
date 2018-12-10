namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}