using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class TimeConverter<T, S> : ICustomJsonMapper<T>
    {
        private Regex regEx = new Regex("^[0-9]{2}:[0-9]{2}:[0-9]{2}[.][0-9]{3}$");
        public int Priority => 3;

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
