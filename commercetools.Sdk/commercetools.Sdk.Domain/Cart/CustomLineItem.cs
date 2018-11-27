using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class CustomLineItem
    {
        public string Id { get; set; }
        public LocalizedString Name { get; set; }
        public Money Money { get; set; }
        public TaxedItemPrice TaxedPrice { get; set; }
        public CentPrecisionMoney TotalPrice { get; set; }
        public double Quantity { get; set; }
        public string Slug { get; set; }
        public ItemState State { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public TaxRate TaxRate { get; set; }
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; set; }
        public CustomFields Custom { get; set; }
        public ItemShippingDetails ShippingDetails { get; set; }
    }
}