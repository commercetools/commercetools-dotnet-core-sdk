using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class AttributeConverter : JsonConverterBase
    {
        private readonly IMapperTypeRetriever<Attribute> mapperTypeRetriever;

        public AttributeConverter(IMapperTypeRetriever<Attribute> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Attribute))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            JToken nameProperty = jsonObject["name"];
            Type genericType = this.mapperTypeRetriever.GetTypeForToken(valueProperty);

            if (genericType == null)
            {
                throw new JsonSerializationException($"Couldn't deserialize attribute '{nameProperty}' value: '{valueProperty}'");
            }

            Type attributeType = typeof(Attribute<>).MakeGenericType(genericType);
            return jsonObject.ToObject(attributeType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
