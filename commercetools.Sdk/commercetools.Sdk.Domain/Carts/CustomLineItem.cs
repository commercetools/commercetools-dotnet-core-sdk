using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.Carts
{
    using System.Collections.Generic;

    public class CustomLineItem
    {
        public string Id { get; set; }
        public LocalizedString Name { get; set; }
        public BaseMoney Money { get; set; }
        public TaxedItemPrice TaxedPrice { get; set; }
        public Money TotalPrice { get; set; }
        public long Quantity { get; set; }
        public string Slug { get; set; }
        public List<ItemState> State { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public TaxRate TaxRate { get; set; }
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; set; }
        public CustomFields Custom { get; set; }
        public ItemShippingDetails ShippingDetails { get; set; }
    }
}
