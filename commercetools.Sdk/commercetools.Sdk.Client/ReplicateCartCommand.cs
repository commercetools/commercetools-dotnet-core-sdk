using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Client
{
    public class ReplicateCartCommand : ReplicateCommand<Cart>
    {
        protected ReplicateCartCommand(IReplicaDraft<Cart> replica)
            : base(replica)
        {
        }

        protected ReplicateCartCommand(IReplicaDraft<Cart> replica, IAdditionalParameters<Cart> additionalParameters)
            : base(replica, additionalParameters)
        {
        }
    }
}