using Newtonsoft.Json.Linq;
using System;

namespace commercetools.Sdk.Serialization
{
    internal abstract class BooleanConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 4;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.Boolean)
            {
                return true;
            }
            return false;
        }
    }
}