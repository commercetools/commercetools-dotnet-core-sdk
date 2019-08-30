using System;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("LineItemStateTransition")]
    public class LineItemStateTransitionMessage : Message<Order>
    {
        public string LineItemId { get; set; }

        public DateTime TransitionDate { get; set; }

        public long Quantity { get; set; }

        public Reference<State> FromState { get; set; }

        public Reference<State> ToState { get; set; }
    }
}
