namespace commercetools.Sdk.Domain
{
    using System.Collections.Generic;

    public class ProductVariantDraft
    {
        public string Sku { get; set; }
        public string Key { get; set; }
        public List<PriceDraft> Prices { get; set; }
        public List<Image> Images { get; set; }
        public List<AssetDraft> Assets { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}