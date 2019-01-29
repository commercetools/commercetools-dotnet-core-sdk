namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public abstract class ProductDiscountValue
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}