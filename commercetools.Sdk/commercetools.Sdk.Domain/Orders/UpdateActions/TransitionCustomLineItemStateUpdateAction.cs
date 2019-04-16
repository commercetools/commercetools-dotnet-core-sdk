using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class TransitionCustomLineItemStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "transitionCustomLineItemState";
        [Required]
        public string CustomLineItemId { get; set; }
        public double Quantity { get; set; }
        public Reference<State> FromState { get; set; }
        public Reference<State> ToState { get; set; }
        public DateTime? ActualTransitionDate { get; set; }
    }
}
