using System;
using System.Collections.Generic;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Domain.Orders
{
    public class ProductVariantImportDraft
    {
        public int? Id { get; set; }
        public string ProductId { get; set; }
        public string Sku { get; set; }
        public List<Price> Prices { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Image> Images { get; set; }


        public ProductVariantImportDraft(string productId, int variantId, List<Price> prices = null,
            List<Attribute> attributes = null, List<Image> images = null)
        {
            Init(prices, attributes, images, productId, variantId);
        }

        public ProductVariantImportDraft(string sku, List<Price> prices = null,
            List<Attribute> attributes = null, List<Image> images = null)
        {
            Init(prices, attributes, images, null, null, sku);
        }

        private void Init(List<Price> prices,
            List<Attribute> attributes, List<Image> images, string productId = null, int? variantId = null,
            string sku = null)
        {
            this.ProductId = productId;
            this.Id = variantId;
            this.Sku = sku;
            this.Prices = prices;
            this.Attributes = attributes;
            this.Images = images;

            // check if both productId and sku are null
            if (string.IsNullOrEmpty(productId) && string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("Pass either productId or sku parameters");
            }
        }
    }
}
