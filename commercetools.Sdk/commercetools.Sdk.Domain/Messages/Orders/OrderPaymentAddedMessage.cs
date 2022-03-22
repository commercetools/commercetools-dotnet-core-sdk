using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Payments;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderPaymentAdded")]
    public class OrderPaymentAddedMessage : Message<Order>
    {
        public Reference<Payment> Payment { get; set; }
    }
}
