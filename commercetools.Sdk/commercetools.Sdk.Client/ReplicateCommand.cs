using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Client
{
    public class ReplicateCommand<T> : Command<Cart>
    {
        public ReplicateCommand(ReplicaCartDraft replica)
        {
            this.Replica = replica;
        }

        public ReplicateCommand(ReplicaCartDraft replica, IAdditionalParameters<Cart> additionalParameters)
        {
            this.Replica = replica;
            this.AdditionalParameters = additionalParameters;
        }

        public ReplicaCartDraft Replica { get; }

        public override System.Type ResourceType => typeof(T);
    }
}