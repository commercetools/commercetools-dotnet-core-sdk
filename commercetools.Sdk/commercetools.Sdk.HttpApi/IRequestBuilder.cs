using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public interface IRequestBuilder
    {
        HttpRequestMessage GetRequestMessageById<T>(Guid guid);
        HttpRequestMessage GetRequestMessageByKey<T>(string guid);
    }
}
