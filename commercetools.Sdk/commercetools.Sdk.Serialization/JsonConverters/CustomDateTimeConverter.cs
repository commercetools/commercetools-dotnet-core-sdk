using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class CustomDateTimeConverter : JsonConverterBase
    {
        private readonly IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter();

        private readonly IsoDateTimeConverter dateOnlyConverter = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy-MM-dd"
        };
        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Serialization };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // In case value does not contain any time span, we are talking about date attributes or custom fields.
            // Therefore, we should not send the time span at all.
            var dateTime = (DateTime)value;
            if (dateTime.TimeOfDay == new TimeSpan(0, 0, 0))
            {
                dateOnlyConverter.WriteJson(writer, value, serializer);
            }
            else
            {
                dateTimeConverter.WriteJson(writer, value, serializer);   
            }
        }
    }
}