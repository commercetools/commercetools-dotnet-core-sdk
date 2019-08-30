using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ParcelItemsUpdated")]
    public class ParcelItemsUpdatedMessage : Message<Order>
    {
        public string DeliveryId { get; set; }
        public string ParcelId { get; set; }
        public List<DeliveryItem> Items { get; set; }
        public List<DeliveryItem> OldItems { get; set; }
    }
}
