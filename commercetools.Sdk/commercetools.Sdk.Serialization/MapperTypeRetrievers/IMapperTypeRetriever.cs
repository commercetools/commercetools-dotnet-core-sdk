using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public interface IMapperTypeRetriever<T>
    {
        Type GetTypeForToken(JToken token);
    }
}
