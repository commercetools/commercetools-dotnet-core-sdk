namespace commercetools.Sdk.Domain
{
    public abstract class CartDiscountValue
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}