using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class AttributeConverter : JsonConverterBase
    {
        private readonly IMapperTypeRetriever<Domain.Attribute> mapperTypeRetriever;

        public AttributeConverter(IMapperTypeRetriever<Domain.Attribute> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Domain.Attribute))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            Type genericType = this.mapperTypeRetriever.GetTypeForToken(valueProperty);

            if (genericType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Attribute type cannot be determined.");
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