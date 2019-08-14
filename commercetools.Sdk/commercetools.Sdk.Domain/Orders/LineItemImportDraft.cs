using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.Orders
{
    public class LineItemImportDraft
    {
        public string ProductId { get; set; }
        public LocalizedString Name { get; set; }
        public ProductVariantImportDraft Variant { get; set; }
        public Price Price { get; set; }
        public long Quantity { get; set; }
        public List<ItemState> State { get; set; }
        public IReference<Channel> SupplyChannel { get; set; }
        public IReference<Channel> DistributionChannel { get; set; }
        public TaxRate TaxRate { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}
