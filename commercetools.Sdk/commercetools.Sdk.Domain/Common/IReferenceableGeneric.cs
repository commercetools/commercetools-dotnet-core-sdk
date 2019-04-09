namespace commercetools.Sdk.Domain
{
    public interface IReferenceable<T>
    {
        string Id { get; }
        ReferenceTypeId TypeId { get; }
        string Key { get; }
    }
}
