namespace commercetools.Sdk.Domain
{
    public class ProductVariantAvailabilityBase
    {
        public bool IsOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
