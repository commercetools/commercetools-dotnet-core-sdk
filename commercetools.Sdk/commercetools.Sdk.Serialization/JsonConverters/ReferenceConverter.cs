using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class ReferenceConverter : JsonConverterBase
    {
        private IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever;

        public ReferenceConverter(IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever)
        {
            this.typeRetriever = typeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Reference) || objectType == typeof(ResourceIdentifier);
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization};

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken typeId = jsonObject["typeId"];

            var type = this.typeRetriever.GetTypeForToken(typeId);
            if (type == null)
            {
                throw new JsonSerializationException();
            }

            var genericReferenceType = typeof(Reference<>).MakeGenericType(type);
            return jsonObject.ToObject(genericReferenceType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
