using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public interface IEndpointRetriever
    {
        string GetEndpoint<T>();
    }
}
