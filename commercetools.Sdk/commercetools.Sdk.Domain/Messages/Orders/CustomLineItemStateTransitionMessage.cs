using System;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("CustomLineItemStateTransition")]
    public class CustomLineItemStateTransitionMessage : Message<Order>
    {
        public string CustomLineItemId { get; set; }

        public DateTime TransitionDate { get; set; }

        public long Quantity { get; set; }

        public Reference<State> FromState { get; set; }

        public Reference<State> ToState { get; set; }
    }

}
