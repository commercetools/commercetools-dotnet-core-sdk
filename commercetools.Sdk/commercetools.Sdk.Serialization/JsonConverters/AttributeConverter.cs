using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Type = System.Type;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using commercetools.Sdk.Serialization.JsonConverters;

namespace commercetools.Sdk.Serialization
{
    internal class AttributeConverter : JsonConverterBase
    {
        private readonly IAttributeConverterReader attributeConverterReader;

        public AttributeConverter(IAttributeConverterReader attributeConverterReader)
        {
            this.attributeConverterReader = attributeConverterReader;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Attribute))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return attributeConverterReader.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
