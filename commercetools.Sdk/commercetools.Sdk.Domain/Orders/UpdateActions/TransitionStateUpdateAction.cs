using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class TransitionStateUpdateAction : OrderUpdateAction
    {
        public override string Action => "transitionState";
        [Required]
        public IReference<State> State { get; set; }
        public bool Force { get; set; }
    }
}