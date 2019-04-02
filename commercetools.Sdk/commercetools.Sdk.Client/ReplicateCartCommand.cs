using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Client
{
    public class ReplicateCartCommand : ReplicateCommand<Cart>
    {
        public ReplicateCartCommand(IReplicaDraft<Cart> replica)
            : base(replica)
        {
        }

        public ReplicateCartCommand(IReplicaDraft<Cart> replica, IAdditionalParameters<Cart> additionalParameters)
            : base(replica, additionalParameters)
        {
        }
    }
}
