using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductStateTransition")]
    public class ProductStateTransitionMessage : Message<Product>
    {
        public Reference<State> State { get; set; }
        public bool Force { get; set; }
    }
}
