using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderLineItemRemoved")]
    public class OrderLineItemRemovedMessage : Message<Order>
    {
        public string LineItemId { get; set; }
        public long RemovedQuantity { get; set; }
        public long NewQuantity { get; set; }
        public List<ItemState> NewState { get; set; }
        public Money NewTotalPrice { get; set; }
        public TaxedItemPrice NewTaxedPrice { get; set; }
        public Price NewPrice { get; set; }
        public ItemShippingDetails NewShippingDetail { get; set; }
    }
}
