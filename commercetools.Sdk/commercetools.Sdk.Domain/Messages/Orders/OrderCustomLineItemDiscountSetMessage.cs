using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderLineItemDiscountSet")]
    public class OrderCustomLineItemDiscountSetMessage : Message<Order>
    {
        public string CustomLineItemId { get; set; }
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; set; }
        public TaxedItemPrice TaxedPrice { get; set; }
    }
}
