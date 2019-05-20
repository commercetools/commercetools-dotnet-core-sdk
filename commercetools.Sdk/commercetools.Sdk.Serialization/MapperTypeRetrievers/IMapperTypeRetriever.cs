using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    public interface IMapperTypeRetriever<T>
    {
        Type GetTypeForToken(JToken token);
    }
}
