namespace commercetools.Sdk.Domain.Common
{
    public interface IVersioned<T> : IIdentifiable<T>
    {
        int Version { get; set; }
    }
}
