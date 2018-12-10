using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class TransitionLineItemStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "transitionLineItemState";
        [Required]
        public string LineItemId { get; set; }
        public double Quantity { get; set; }
        public Reference<State> FromState { get; set; }
        public Reference<State> ToState { get; set; }
        public DateTime ActualTransitionDate { get; set; }
    }
}