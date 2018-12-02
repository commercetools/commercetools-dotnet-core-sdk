namespace commercetools.Sdk.Domain
{
    public abstract class SuggestTokenizer
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}