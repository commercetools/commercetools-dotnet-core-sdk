namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public interface IRequestMessageBuilder
    {
        Type CommandType { get; }
        HttpRequestMessage GetRequestMessage<T>(ICommand command);
    }
}