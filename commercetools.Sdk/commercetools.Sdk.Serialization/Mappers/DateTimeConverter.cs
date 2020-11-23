using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class DateTimeConverter<T, S> : ICustomJsonMapper<T>
    {
        private Regex regEx = new Regex("^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}.?[0-9]+Z$");
        
        public int Priority => 4;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                return regEx.IsMatch(property.Value<string>()); 
            }
            return false;
        }
    }
}