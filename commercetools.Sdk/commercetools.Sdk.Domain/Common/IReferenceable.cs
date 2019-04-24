namespace commercetools.Sdk.Domain.Common
{
    public interface IReferenceable<T>
    {
        IReference<T> ToReference();
    }
}
