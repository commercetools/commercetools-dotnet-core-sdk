using System;

namespace commercetools.Sdk.Domain.Orders
{
    public abstract class ReturnItem
    {
        public string Type => this.GetType().GetTypeMarkerAttributeValue();
        public string Id { get; set; }
        public long Quantity { get; set; }
        public string Comment { get; set; }
        public ReturnShipmentState ShipmentState { get; set; }
        public ReturnPaymentState PaymentState { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
