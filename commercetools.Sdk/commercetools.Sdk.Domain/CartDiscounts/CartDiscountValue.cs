namespace commercetools.Sdk.Domain.CartDiscounts
{
    public abstract class CartDiscountValue
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}