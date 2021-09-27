using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class DateTimeSerializationConverter : JsonConverterBase
    {
        private IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
        private SerializeDateTimeFormatAttribute serializeDateTimeFormatAttribute;

        public override List<SerializerType> SerializerTypes =>
            new List<SerializerType>() {SerializerType.Serialization};

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime)
                && objectType.IsDefined(typeof(SerializeDateTimeFormatAttribute)))
            {
                serializeDateTimeFormatAttribute = objectType.GetCustomAttribute<SerializeDateTimeFormatAttribute>();
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTimeFormat = serializeDateTimeFormatAttribute.DateTimeFormat;
            isoDateTimeConverter.DateTimeFormat = dateTimeFormat;
            isoDateTimeConverter.WriteJson(writer, value, serializer);
        }
    }
}