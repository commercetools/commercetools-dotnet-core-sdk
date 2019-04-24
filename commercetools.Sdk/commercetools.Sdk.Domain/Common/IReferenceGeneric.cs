namespace commercetools.Sdk.Domain.Common
{
    public interface IReference<T> : IReferenceable<T>, IIdentifiable<T>
    {
        string Id { get; }
        ReferenceTypeId TypeId { get; }
        string Key { get; }
    }
}
