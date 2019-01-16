using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    internal interface IMapperTypeRetriever<T>
    {
        Type GetTypeForToken(JToken token);
    }
}