using Newtonsoft.Json.Linq;
using System;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class TimeConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 3;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                if (TimeSpan.TryParseExact(property.Value<string>(), @"hh\:mm\:ss\.fff", null, out TimeSpan time))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
