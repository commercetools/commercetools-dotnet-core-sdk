using commercetools.Sdk.Client;
using System;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public interface IRequestMessageBuilder
    {
        HttpMethod HttpMethod { get; }
        string RequestUriEnd { get; }
        object RequestBody { get; }
    }
}