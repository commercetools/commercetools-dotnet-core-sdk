using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Subscriptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class SubscriptionPayloadConverter : JsonConverterBase
    {
        private readonly IDecoratorTypeRetriever<Payload> decoratorTypeRetriever;
        private IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever;
        public string PropertyName => "notificationType";

        public Type DefaultType => typeof(GeneralPayload);

        public SubscriptionPayloadConverter(IDecoratorTypeRetriever<Payload> decoratorTypeRetriever, IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
            this.typeRetriever = typeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Payload) || (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Payload<>)))
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
                return jsonObject.ToObject(propertyType, serializer);
            }
            // create generic payload based on resource (like if resource is customer, then object type
            // will be like ResourceCreatedPayload<Customer> or MessageSubscriptionPayload<Customer>
            if (jsonObject["resource"]?["typeId"] is JToken typeIdToken)
            {
                var resourceType = this.typeRetriever.GetTypeForToken(typeIdToken);
                var genericPayloadType = propertyType.MakeGenericType(resourceType);
                return jsonObject.ToObject(genericPayloadType, serializer);
            }
            throw new JsonSerializationException($"Unknown subscription payload type: {jsonObject.ToString(Formatting.None)}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
