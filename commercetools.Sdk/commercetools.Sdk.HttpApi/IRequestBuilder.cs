using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public interface IRequestBuilder
    {
        HttpRequestMessage GetRequestMessage<T>(ICommand command);
    }
}
