using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public interface IQueryStringRequestBuilder<T> : IQueryStringRequestBuilder
    {
        List<KeyValuePair<string, string>> GetQueryStringParameters(IAdditionalParameters<T> additionalParameters);
    }
}
