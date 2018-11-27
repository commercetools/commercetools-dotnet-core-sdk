using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    public interface ICustomJsonMapper<T>
    {
        bool CanConvert(JToken property);

        int Priority { get; }
        Type Type { get; }
    }
}