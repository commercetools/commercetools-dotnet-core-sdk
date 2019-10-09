using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Serialization
{
    internal abstract class JsonConverterDecoratorTypeRetrieverBase<T> : JsonConverterBase
    {
        private readonly IDecoratorTypeRetriever<T> decoratorTypeRetriever;
        public abstract string PropertyName { get; }
        public virtual Type DefaultType { get => null; }

        public JsonConverterDecoratorTypeRetrieverBase(IDecoratorTypeRetriever<T> decoratorTypeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(T))
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
                if (this.DefaultType != null)
                {
                    propertyType = this.DefaultType;
                }
                else
                {
                    throw new JsonSerializationException($"Couldn't get type to deserialize property {this.PropertyName}");
                }
            }

            return jsonObject.ToObject(propertyType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
