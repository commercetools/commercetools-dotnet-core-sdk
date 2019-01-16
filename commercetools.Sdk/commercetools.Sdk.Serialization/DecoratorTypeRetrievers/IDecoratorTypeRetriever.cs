using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    internal interface IDecoratorTypeRetriever<T>
    {
        Type GetTypeForToken(JToken token);
    }
}