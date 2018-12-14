using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public class AttributeEnumConverter : JsonConverterBase
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EnumAttribute)
                   || objectType == typeof(LocalizedEnumAttribute);
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Serialization };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        { 
            var result = new
            {
                Value = GetKey(value),
                Name = GetName(value)
            };
            serializer.Serialize(writer, result);
        }

        private static string GetKey(object value)
        {
            return value.GetType() == typeof(EnumAttribute) ? ((EnumAttribute) value).Value.Key : ((LocalizedEnumAttribute)value).Value.Key;
        }

        private static string GetName(object value)
        {
            return value.GetType() == typeof(EnumAttribute) ? ((EnumAttribute)value).Name : ((LocalizedEnumAttribute)value).Name;
        }
    }
}