using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public interface IQueryStringRequestBuilderFactory
    {
        IQueryStringRequestBuilder<T> GetQueryStringRequestBuilder<T>();
    }
}
