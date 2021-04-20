using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class EnumConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 3;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues && !property.IsArrayToken())
            {
                var keyProperty = property["key"];
                var labelProperty = property["label"];
                if (keyProperty != null && labelProperty != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}