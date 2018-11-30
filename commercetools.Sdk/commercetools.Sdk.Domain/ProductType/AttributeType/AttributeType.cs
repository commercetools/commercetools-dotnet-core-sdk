namespace commercetools.Sdk.Domain
{
    public abstract class AttributeType
    {
        public string Name
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}