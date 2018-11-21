using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public interface IDecoratorTypeRetriever<T>
    {
        Type GetTypeForToken(JToken token);
    }
}
