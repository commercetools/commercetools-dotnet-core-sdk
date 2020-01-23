using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class TransitionLineItemStateStagedAction : StagedOrderUpdateAction
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
