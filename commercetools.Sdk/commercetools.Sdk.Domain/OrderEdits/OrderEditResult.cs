namespace commercetools.Sdk.Domain.OrderEdits
{
    public abstract class OrderEditResult
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}