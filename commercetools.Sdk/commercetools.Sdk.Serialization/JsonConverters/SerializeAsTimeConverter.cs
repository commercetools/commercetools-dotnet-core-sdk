using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    [Obsolete("Experimental")]
    internal class SerializeAsTimeConverter : JsonConverterBase
    {
        private readonly IsoDateTimeConverter timeOnlyConverter = new IsoDateTimeConverter
        {
            DateTimeFormat = "HH:mm:ss"
        };

        private string[] TimeOnlyProps { get; set; }

        public override List<SerializerType> SerializerTypes =>
            new List<SerializerType>() {SerializerType.Serialization};

        public override bool CanConvert(Type objectType)
        {
            var hasDateOnlyProps = objectType.IsDefined(typeof(SerializeAsTimeOnlyAttribute));
            if (hasDateOnlyProps)
            {
                var attribute = objectType.GetCustomAttribute<SerializeAsTimeOnlyAttribute>();
                TimeOnlyProps = attribute.Properties;
            }

            return hasDateOnlyProps;
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
                    if (TimeOnlyProps.Contains(prop.Name, StringComparer.CurrentCultureIgnoreCase)
                        &&
                        propValue is DateTime valueDate)
                    {
                        timeOnlyConverter.WriteJson(writer, valueDate, serializer);
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