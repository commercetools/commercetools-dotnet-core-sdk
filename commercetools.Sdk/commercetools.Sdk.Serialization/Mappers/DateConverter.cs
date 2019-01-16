using Newtonsoft.Json.Linq;
using System;

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
                if (DateTime.TryParse(property.Value<string>(), out DateTime date))
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