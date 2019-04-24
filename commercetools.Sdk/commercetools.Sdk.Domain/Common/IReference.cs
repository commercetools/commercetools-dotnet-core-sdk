namespace commercetools.Sdk.Domain.Common
{
    public interface IReference
    {
        string Id { get; }
        ReferenceTypeId? TypeId { get; }
        string Key { get; }
    }
}
