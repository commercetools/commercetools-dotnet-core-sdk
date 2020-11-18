using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Serialization
{
    internal class CustomDateTimeConverter : JsonConverterBase
    {
        private IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Serialization };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // In case value does not contain any time span, we are talking about date attributes or custom fields.
            // Therefore, we should not send the time span at all.
            DateTime dateTime = (DateTime)value;
            if (dateTime.TimeOfDay == new TimeSpan(0, 0, 0))
            {
                isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd";
            }
            isoDateTimeConverter.WriteJson(writer, value, serializer);
        }
    }
}