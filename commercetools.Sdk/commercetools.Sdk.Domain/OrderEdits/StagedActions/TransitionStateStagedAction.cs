using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class TransitionStateStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "transitionState";
        [Required]
        public IReference<State> State { get; set; }
        public bool Force { get; set; }
    }
}