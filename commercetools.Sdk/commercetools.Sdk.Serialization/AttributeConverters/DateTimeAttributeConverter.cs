using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class DateTimeAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 4;

        public Type Type => typeof(DateTimeAttribute);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.Date)
            {
                return true;
            }
            return false;
        }
    }
}
