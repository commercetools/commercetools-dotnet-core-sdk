using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain.Carts
{
    public class LineItemDraft : IDraft<LineItem>
    {
        public int? VariantId { get; set; }

        public string ProductId { get; set; }

        public string Sku { get; set; }

        public long Quantity { get; set; }

        public Reference<Channel> SupplyChannel { get; set; }

        public Reference<Channel> DistributionChannel { get; set; }

        public ExternalTaxRateDraft ExternalTaxRate { get; set; }

        public BaseMoney ExternalPrice { get; set; }

        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }

        public CustomFieldsDraft Custom { get; set; }

        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}
