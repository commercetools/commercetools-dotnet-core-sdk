using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderLineItemDiscountSet")]
    public class OrderLineItemDiscountSetMessage : Message<Order>
    {
        public string LineItemId { get; set; }
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; set; }
        public Money TotalPrice { get; set; }
        public TaxedItemPrice TaxedPrice { get; set; }
    }
}
