using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class TransitionStateUpdateAction : UpdateAction<Payment>
    {
        public string Action => "transitionState";
        [Required]
        public Reference<State> State { get; set; }
        public bool Force { get; set; }
    }
}