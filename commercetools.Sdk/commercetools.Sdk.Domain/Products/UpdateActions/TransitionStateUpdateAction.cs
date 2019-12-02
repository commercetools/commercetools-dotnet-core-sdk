using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class TransitionStateUpdateAction : UpdateAction<Product>
    {
        public string Action => "transitionState";
        public IReference<State> State { get; set; }
        public bool Force { get; set; }
    }
}