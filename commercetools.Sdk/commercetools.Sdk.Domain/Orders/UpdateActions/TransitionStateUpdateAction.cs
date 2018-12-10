using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class TransitionStateUpdateAction : UpdateAction<Order>
    {
        public string Action => "transitionState";
        [Required]
        public Reference<State> State { get; set; }
        public bool Force { get; set; }
    }
}