using System;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("LineItemStateTransition")]
    public class LineItemStateTransitionMessage : Message
    {
        public string LineItemId { get;}

        public DateTime TransitionDate { get;}

        public int Quantity { get;}
        
        public Reference<State> FromState { get;}

        public Reference<State> ToState { get;}
    }
}