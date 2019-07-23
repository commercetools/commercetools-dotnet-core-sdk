namespace commercetools.Sdk.Domain.CartDiscounts
{
    public abstract class CartDiscountTarget
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}