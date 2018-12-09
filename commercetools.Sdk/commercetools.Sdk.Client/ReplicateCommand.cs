namespace commercetools.Sdk.Client
{
    using Domain.Carts;

    // TODO Implement request builder
    public class ReplicateCommand<T> : Command<Cart>
    {
        public ReplicateCommand(ReplicaCartDraft replica)
        {
            this.Replica = replica;
        }

        public ReplicaCartDraft Replica { get; }

        public override System.Type ResourceType => typeof(T);
    }
}