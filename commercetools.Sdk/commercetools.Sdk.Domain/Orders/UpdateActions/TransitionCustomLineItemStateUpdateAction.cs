using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class TransitionCustomLineItemStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "transitionCustomLineItemState";
        [Required]
        public string CustomLineItemId { get; set; }
        public long Quantity { get; set; }
        public Reference<State> FromState { get; set; }
        public Reference<State> ToState { get; set; }
        public DateTime? ActualTransitionDate { get; set; }
    }
}
