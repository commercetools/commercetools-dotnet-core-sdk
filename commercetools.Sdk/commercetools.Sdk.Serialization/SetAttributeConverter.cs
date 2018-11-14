using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class SetAttributeConverter : JsonConverter
    {
        private readonly IMapperTypeRetriever<Domain.Attribute> mapperTypeRetriever;

        public SetAttributeConverter(IMapperTypeRetriever<Domain.Attribute> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(SetAttribute<>))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject[0];

            Type attributeType;  attributeType = this.mapperTypeRetriever.GetTypeForToken(valueProperty);            

            if (attributeType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Attribute type cannot be determined.");
            }

            Type setType = typeof(SetAttribute<>);
            setType.MakeGenericType(attributeType);

            return jsonObject.ToObject(setType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private bool IsSetAttribute(JToken valueProperty)
        {
            if (valueProperty != null)
            {
                return valueProperty.HasValues && valueProperty.Type == JTokenType.Array;
            }
            return false;
        }
    }
}