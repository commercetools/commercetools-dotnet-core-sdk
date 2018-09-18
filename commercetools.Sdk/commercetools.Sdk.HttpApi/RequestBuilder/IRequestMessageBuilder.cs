namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public interface IRequestMessageBuilder
    {
        HttpRequestMessage GetRequestMessage<T>(ICommand<T> command);
    }
}