using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public abstract class StringConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 4;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                return true;
            }
            return false;
        }
    }
}