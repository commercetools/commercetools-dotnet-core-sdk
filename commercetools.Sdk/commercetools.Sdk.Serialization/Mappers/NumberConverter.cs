using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class NumberConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 4;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.Integer || property?.Type == JTokenType.Float)
            {
                return true;
            }
            return false;
        }
    }
}