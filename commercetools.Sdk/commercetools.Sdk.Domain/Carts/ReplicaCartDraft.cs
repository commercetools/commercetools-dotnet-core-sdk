namespace commercetools.Sdk.Domain.Carts
{
    public class ReplicaCartDraft : IReplicaDraft<Cart>
    {
        public Reference Reference { get; set; }
        public string Key { get; set; }
    }
}