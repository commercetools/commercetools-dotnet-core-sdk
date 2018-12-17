using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class EnumConverter : JsonConverterBase
    {
        public override bool CanConvert(Type objectType)
        {
            if (typeof(Enum).IsAssignableFrom(objectType))
            {
                return true;
            }
            return false;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization, SerializerType.Serialization };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;
            return enumString.GetEnum(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enum e = (Enum)value;
            string result = e.GetDescription();
            writer.WriteValue(result);
        }
    }
}