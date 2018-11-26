using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FieldTypeConverter : JsonConverterBase
    {
        private readonly IDecoratorTypeRetriever<FieldType> decoratorTypeRetriever;

        public FieldTypeConverter(IDecoratorTypeRetriever<FieldType> decoratorTypeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(FieldType))
            {
                return true;
            }
            return false;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken nameProperty = jsonObject["name"];
            
            Type fieldType = this.decoratorTypeRetriever.GetTypeForToken(nameProperty);

            if (fieldType == null)
            {
                throw new JsonSerializationException("Field type cannot be determined.");
            }

            return jsonObject.ToObject(fieldType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }           
    }
}