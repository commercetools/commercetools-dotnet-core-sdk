using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    internal class MessageConverter : JsonConverterBase
    {
        private readonly IDecoratorTypeRetriever<Message> decoratorTypeRetriever;
        public string PropertyName => "type";

        public Type DefaultType => typeof(GeneralMessage);


        public MessageConverter(IDecoratorTypeRetriever<Message> decoratorTypeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Message) || (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Message<>)))
            {
                return true;
            }
            return false;
        }
        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken property = jsonObject[this.PropertyName];

            Type propertyType = this.decoratorTypeRetriever.GetTypeForToken(property);

            if (propertyType == null)
            {
                propertyType = this.DefaultType;
            }

            return jsonObject.ToObject(propertyType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
