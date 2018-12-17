namespace commercetools.Sdk.Domain
{
    public abstract class CartDiscountTarget
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}