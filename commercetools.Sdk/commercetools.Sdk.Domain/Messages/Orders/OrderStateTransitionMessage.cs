using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.States;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderStateTransition")]
    public class OrderStateTransitionMessage : Message<Order>
    {
        public Reference<State> State { get; set; }
        public Reference<State> OldState { get; set; }
        public bool Force { get; set; }
    }
}