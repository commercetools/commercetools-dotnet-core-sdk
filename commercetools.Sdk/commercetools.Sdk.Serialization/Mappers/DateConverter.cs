using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace commercetools.Sdk.Serialization
{
    internal abstract class DateConverter<T, S> : ICustomJsonMapper<T>
    {
        private Regex regEx = new Regex("^[0-9]{4}-[0-9]{2}-[0-9]{2}$");
        public int Priority => 2;

        public Type Type => typeof(S);

        public virtual bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                return regEx.IsMatch(property.Value<string>()); 
            }
            return false;
        }
    }
}
