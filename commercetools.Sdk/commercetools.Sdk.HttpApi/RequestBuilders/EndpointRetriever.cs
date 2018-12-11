using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class EndpointRetriever : IEndpointRetriever
    {
        public string GetEndpoint<T>()
        {
            return typeof(T).GetEndpointValue();
        }
    }
}
