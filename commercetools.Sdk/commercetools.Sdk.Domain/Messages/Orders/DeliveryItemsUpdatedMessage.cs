using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("DeliveryItemsUpdated")]
    public class DeliveryItemsUpdatedMessage : Message<Order>
    {
        public string DeliveryId { get; set; }
        public List<DeliveryItem> Items { get; set; }
        public List<DeliveryItem> OldItems { get; set; }
    }
}
