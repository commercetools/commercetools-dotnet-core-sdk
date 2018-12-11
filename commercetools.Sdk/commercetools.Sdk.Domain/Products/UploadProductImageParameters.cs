namespace commercetools.Sdk.Domain.Products
{
    public class UploadProductImageParameters : IUploadImageParameters<Product>
    {
        public int? Variant { get; set; }

        public string Sku { get; set; }

        public string Filename { get; set; }

        public bool? Staged { get; set; }
    }
}
