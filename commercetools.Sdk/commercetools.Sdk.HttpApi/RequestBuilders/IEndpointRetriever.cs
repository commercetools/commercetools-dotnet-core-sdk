using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public interface IEndpointRetriever
    {
        string GetEndpoint<T>();
    }
}
