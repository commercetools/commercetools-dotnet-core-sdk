using System.Collections.Generic;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Key { get; set; }        
        public List<Price> Prices { get; set; }
        public List<Attribute> Attributes { get; set; }
        public Price Price { get; set; }
        public List<Image> Images { get; set; }
        public List<Asset> Assets { get; set; }
        public ProductVariantAvailability Availability { get; set; }
        public bool IsMatchingVariant { get; set; }
        public ScopedPrice ScopedPrice { get; set; }
        public bool ScopedPriceDiscounted { get; set; }
    }
}