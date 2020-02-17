using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class TransitionLineItemStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "transitionLineItemState";
        [Required]
        public string LineItemId { get; set; }
        public long Quantity { get; set; }
        public Reference<State> FromState { get; set; }
        public Reference<State> ToState { get; set; }
        public DateTime? ActualTransitionDate { get; set; }
    }
}
