using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class DictionaryConverter : JsonConverterBase
    {
        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Serialization };

        public override bool CanConvert(Type objectType)
        {
            if (typeof(IDictionary<string, object>).IsAssignableFrom(objectType))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IDictionary<string, object> values)
            {
                writer.WriteStartObject();
                foreach (var field in values)
                {
                    writer.WritePropertyName(field.Key);
                    serializer.Serialize(writer, field.Value);
                }
                writer.WriteEndObject();
            }
        }
    }
}
