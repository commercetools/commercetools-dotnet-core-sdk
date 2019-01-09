using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class ReplicateCommand<T> : Command<T>
    {
        protected ReplicateCommand(IReplicaDraft<T> replica)
        {
            this.Replica = replica;
        }

        protected ReplicateCommand(IReplicaDraft<T> replica, IAdditionalParameters<T> additionalParameters)
        {
            this.Replica = replica;
            this.AdditionalParameters = additionalParameters;
        }

        public IReplicaDraft<T> Replica { get; }

        public override System.Type ResourceType => typeof(T);
    }
}