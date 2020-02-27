using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi
{
    public interface ICtpClientFactory
    {
        IClient Create(string name);
    }
}