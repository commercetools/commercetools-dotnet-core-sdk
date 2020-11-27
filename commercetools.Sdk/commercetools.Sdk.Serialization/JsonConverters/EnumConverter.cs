using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class EnumConverter : JsonConverterBase
    {
        private readonly ConcurrentDictionary<string, object> enumTypes = new ConcurrentDictionary<string, object>();
        
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
            var enumKey = enumString + objectType.FullName;
            return enumTypes.GetOrAdd(enumKey, enumValue =>
            {
                return enumString.GetEnum(objectType);
            });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enum e = (Enum) value;
            string result = e.GetDescription();
            writer.WriteValue(result);
        }
    }
}
