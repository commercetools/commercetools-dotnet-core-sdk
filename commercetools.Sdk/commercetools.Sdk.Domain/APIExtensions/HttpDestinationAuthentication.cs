namespace commercetools.Sdk.Domain.APIExtensions
{
    public abstract class HttpDestinationAuthentication
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}
