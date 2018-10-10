using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public class DateAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 2;

        public Type Type => typeof(DateAttribute);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                DateTime date;
                // TODO See in which format the date is saved and if there are localizations
                if (DateTime.TryParse(property.Value<string>(), out date))
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
