namespace commercetools.Sdk.Domain.Subscriptions
{
    public abstract class Format
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}