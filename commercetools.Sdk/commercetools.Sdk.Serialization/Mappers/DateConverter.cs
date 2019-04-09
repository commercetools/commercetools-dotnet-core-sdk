using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace commercetools.Sdk.Serialization
{
    internal abstract class DateConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 2;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                if (DateTime.TryParseExact(property.Value<string>(), "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime date))
                {
                    if (date.TimeOfDay.Ticks == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
