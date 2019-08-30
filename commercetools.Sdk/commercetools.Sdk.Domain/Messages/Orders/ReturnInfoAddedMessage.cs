using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ReturnInfoAdded")]
    public class ReturnInfoAddedMessage : Message<Order>
    {
        public ReturnInfo ReturnInfo { get; set; }
    }
}
