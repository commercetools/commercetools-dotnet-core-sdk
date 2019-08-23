namespace commercetools.Sdk.Domain.Subscriptions.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Subscription>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
