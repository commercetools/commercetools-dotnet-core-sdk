using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderDeleted")]
    public class OrderDeletedMessage : Message<Order>
    {
        public Order Order { get; set; }
    }
}
