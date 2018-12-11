namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public interface IRequestMessageBuilderFactory
    {
        T GetRequestMessageBuilder<T>();
    }
}