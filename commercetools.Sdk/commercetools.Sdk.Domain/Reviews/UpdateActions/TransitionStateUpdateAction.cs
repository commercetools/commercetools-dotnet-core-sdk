using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Reviews
{
    public class TransitionStateUpdateAction : UpdateAction<Review>
    {
        public string Action => "transitionState";
        public IReference<State> State { get; set; }
        public bool Force { get; set; }
    }
}
