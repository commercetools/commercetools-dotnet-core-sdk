using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class IReferenceConverter : JsonConverterBase
    {
        private IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever;

        public IReferenceConverter(IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever)
        {
            this.typeRetriever = typeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(IReference<>);
        }

        public override List<SerializerType> SerializerTypes =>
            new List<SerializerType>() {SerializerType.Deserialization};

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken typeId = jsonObject["typeId"];
            var isKeyReferencable = jsonObject["key"] != null;


            Type type = null;
            try
            {
                type = this.typeRetriever.GetTypeForToken(typeId);
            }
            catch (Exception)
            {
                // ignored
            }

            if (type == null)
            {
                throw new JsonSerializationException($"Unknown reference typeId '{typeId}'");
            }

            var deserializedType = isKeyReferencable
                ? typeof(ResourceIdentifier<>)
                : typeof(Reference<>);

            var genericReferenceType = deserializedType.MakeGenericType(type);
            var deserialized = jsonObject.ToObject(genericReferenceType, serializer);
            return deserialized;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}