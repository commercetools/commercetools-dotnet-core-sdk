using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Orders
{
    public class ProductVariantImportDraft
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public List<Price> Prices { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Image> Images { get; set; }
    }
}