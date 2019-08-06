using System.Collections.Generic;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.Carts
{
    public class LineItem
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString ProductSlug { get; set; }
        public Reference<ProductType> ProductType { get; set; }
        public ProductVariant Variant { get; set; }
        public Price Price { get; set; }
        public TaxedItemPrice TaxedPrice { get; set; }
        public Money TotalPrice { get; set; }
        public long Quantity { get; set; }
        public List<ItemState> State { get; set; }
        public TaxRate TaxRate { get; set; }
        public Reference<Channel> SupplyChannel { get; set; }
        public Reference<Channel> DistributionChannel { get; set; }
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; set; }
        public LineItemPriceMode PriceMode { get; set; }
        public LineItemMode LineItemMode { get; set; }
        public CustomFields Custom { get; set; }
        public ItemShippingDetails ShippingDetails { get; set; }
    }
}
