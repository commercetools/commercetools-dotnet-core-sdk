using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class ReferenceConverter : JsonConverterBase
    {
        private IDecoratorTypeRetriever<ResourceTypeAttribute> typeRetriever;
        private ConcurrentDictionary<Type, Type> referenceTypes = new ConcurrentDictionary<Type, Type>();
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

            var genericReferenceType = referenceTypes.GetOrAdd(type, (t) => { return typeof(Reference<>).MakeGenericType(t); });
            return jsonObject.ToObject(genericReferenceType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
