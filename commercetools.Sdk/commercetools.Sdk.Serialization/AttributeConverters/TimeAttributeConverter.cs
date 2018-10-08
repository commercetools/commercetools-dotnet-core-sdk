using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public class TimeAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 3;

        public Type Type => typeof(TimeAttribute);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                DateTime time;
                if (DateTime.TryParse(property.Value<string>(), out time))
                {
                    if (time.TimeOfDay.Ticks != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
