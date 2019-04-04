namespace commercetools.Sdk.Domain
{
    public interface IReferenceable
    {
        string Id { get; }
        ReferenceTypeId TypeId { get; }
        string Key { get; }
    }
}
