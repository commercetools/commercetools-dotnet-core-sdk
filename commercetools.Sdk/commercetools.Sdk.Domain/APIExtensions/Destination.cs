namespace commercetools.Sdk.Domain.APIExtensions
{
    public abstract class Destination
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}
