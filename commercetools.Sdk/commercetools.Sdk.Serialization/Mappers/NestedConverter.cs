using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class NestedConverter<T, S> : ICustomJsonMapper<T>
    {

        protected NestedConverter()
        {
        }

        public int Priority => 1;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            return property != null && property.IsNestedAttribute();
        }
    }
}
