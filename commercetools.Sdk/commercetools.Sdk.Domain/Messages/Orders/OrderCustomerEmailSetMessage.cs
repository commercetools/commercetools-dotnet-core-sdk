using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCustomerEmailSet")]
    public class OrderCustomerEmailSetMessage : Message<Order>
    {
        public string Email { get; set; }
        public string OldEmail { get; set; }
    }
}
