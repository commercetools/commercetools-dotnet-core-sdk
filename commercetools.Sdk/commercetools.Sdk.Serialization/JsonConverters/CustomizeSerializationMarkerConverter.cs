using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class CustomizeSerializationMarkerConverter : JsonConverterBase
    {
        private readonly IsoDateTimeConverter dateOnlyConverter = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy-MM-dd"
        };
        public override List<SerializerType> SerializerTypes =>
            new List<SerializerType>() {SerializerType.Serialization};

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsDefined(typeof(CustomizeSerializationMarkerAttribute));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object objectValue, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var prop in objectValue.GetType().GetProperties())
            {
                var propValue = prop.GetValue(objectValue);
                if (propValue != null)
                {
                    writer.WritePropertyName(prop.Name.ToCamelCase());
                    if (prop.GetCustomAttribute<AsDateOnlyAttribute>()!= null
                        &&
                        propValue is DateTime valueDate)
                    {
                        dateOnlyConverter.WriteJson(writer, valueDate, serializer);
                    }
                    else
                    {
                        serializer.Serialize(writer, propValue);
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}