namespace commercetools.Sdk.Domain.Messages.Products
{
    public class ProductPriceDiscountsSetUpdatedPrice
    {
        public int VariantId { get; set;}

        public string VariantKey { get; set;}

        public string Sku { get; set;}

        public string PriceId { get; set;}

        public DiscountedPrice Discounted { get; set;}

        public bool Staged { get; set;}
    }
}
