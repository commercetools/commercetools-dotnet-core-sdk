using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi
{
    public interface IRequestMessageBuilderFactory
    {
        T GetRequestMessageBuilder<T>();
    }
}