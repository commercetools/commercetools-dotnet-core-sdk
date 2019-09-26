using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.Subscriptions;
using commercetools.Sdk.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    /// <summary>
    /// this converter to wrap all message properties into message object
    /// </summary>
    internal class MessageSubscriptionPayloadConverter : JsonConverterBase
    {
        private IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever;

        public MessageSubscriptionPayloadConverter(IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever)
        {
            this.typeRetriever = typeRetriever;
        }

        public override List<SerializerType> SerializerTypes =>
            new List<SerializerType>() {SerializerType.Deserialization};

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsGenericType &&
                objectType.GetGenericTypeDefinition() == typeof(MessageSubscriptionPayload<>))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            if (jsonObject["resource"]["typeId"] is JToken typeIdToken)
            {
                var resourceType = this.typeRetriever.GetTypeForToken(typeIdToken);
                var messageSubscriptionPayloadType = typeof(MessageSubscriptionPayload<>).MakeGenericType(resourceType);
                var subscriptionPayload = Activator.CreateInstance(messageSubscriptionPayloadType);
                var properties = messageSubscriptionPayloadType.GetProperties();

                //deserialize it as generic message
                var messageType = typeof(Message<>).MakeGenericType(resourceType);
                var message = jsonObject.ToObject(messageType, serializer);

                foreach (var field in properties)
                {
                    var name = field.Name.ToCamelCase();
                    var propType = field.PropertyType;
                    if (name == "message")
                    {
                        field.SetValue(subscriptionPayload, message);
                    }
                    else if (name != "notificationType" && jsonObject[name] != null)
                    {
                        var fieldValue = jsonObject[name].ToObject(propType);
                        field.SetValue(subscriptionPayload, fieldValue);
                    }
                }

                return subscriptionPayload;
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}