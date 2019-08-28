namespace commercetools.Sdk.Domain.Subscriptions
{
    public abstract class Destination
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}
