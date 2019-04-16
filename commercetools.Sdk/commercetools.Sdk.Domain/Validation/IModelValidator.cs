namespace commercetools.Sdk.Domain.Validation
{
    public interface IModelValidator
    {
        void Validate<T>(T Object);
    }
}
