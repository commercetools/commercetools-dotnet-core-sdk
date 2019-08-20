using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class FieldsConverter : JsonConverterBase
    {
        private readonly IMapperTypeRetriever<Fields> mapperTypeRetriever;

        public FieldsConverter(IMapperTypeRetriever<Fields> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization, SerializerType.Serialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Fields))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Fields customFields = new Fields();
            JObject jsonObject = JObject.Load(reader);
            if (jsonObject.Count == 0)
            {
                return customFields;
            }

            foreach (JProperty property in jsonObject.Children())
            {
                string key = property.Name;
                JToken value = property.Value;
                Type valueType = this.mapperTypeRetriever.GetTypeForToken(value);
                object o = value.ToObject(valueType, serializer);
                customFields.Add(key, o);
            }

            return customFields;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Fields fields)
            {
                writer.WriteStartObject();
                foreach (var field in fields)
                {
                    writer.WritePropertyName(field.Key);
                    serializer.Serialize(writer, field.Value);
                }
                writer.WriteEndObject();
            }

        }
    }
}
