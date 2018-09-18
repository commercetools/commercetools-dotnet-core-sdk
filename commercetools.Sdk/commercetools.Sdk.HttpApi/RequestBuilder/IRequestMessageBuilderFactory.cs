using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi
{
    public interface IRequestMessageBuilderFactory
    {
        IRequestMessageBuilder GetRequestMessageBuilder<T>(ICommand<T> command);
    }
}