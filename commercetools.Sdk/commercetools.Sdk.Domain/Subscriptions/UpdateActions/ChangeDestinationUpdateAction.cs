namespace commercetools.Sdk.Domain.Subscriptions.UpdateActions
{
    public class ChangeDestinationUpdateAction : UpdateAction<Subscription>
    {
        public string Action => "setKey";
        public Destination Destination { get; set; }
    }
}
