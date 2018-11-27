using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    public interface IDecoratorTypeRetriever<T>
    {
        Type GetTypeForToken(JToken token);
    }
}