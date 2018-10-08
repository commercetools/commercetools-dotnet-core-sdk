using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    public interface ICustomConverter<T>
    {
        bool CanConvert(JToken property);
        int Priority { get; }
        Type Type { get; }
    }
}