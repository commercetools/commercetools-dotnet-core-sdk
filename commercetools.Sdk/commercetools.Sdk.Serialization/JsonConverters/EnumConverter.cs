using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class EnumConverter : JsonConverterBase
    {
        public override bool CanConvert(Type objectType)
        {
            if (typeof(Enum).IsAssignableFrom(objectType))
            {
                return true;
            }

            var t = Nullable.GetUnderlyingType(objectType);
            if (t != null && t.IsEnum)
            {
                return true;
            }

            return false;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>()
            {SerializerType.Deserialization, SerializerType.Serialization};

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var enumString = (string) reader.Value;
            return enumString.GetEnum(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enum e = (Enum) value;
            string result = e.GetDescription();
            writer.WriteValue(result);
        }
    }
}
