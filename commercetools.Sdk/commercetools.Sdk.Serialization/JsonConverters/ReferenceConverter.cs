using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class ReferenceConverter : JsonConverterBase
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Reference);
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization, SerializerType.Serialization };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken typeId = jsonObject["typeId"];
            var enumVal = (ReferenceTypeId)typeId.ToString().GetEnum(typeof(ReferenceTypeId));

            var t = enumVal.GetResourceType();

            var referenceType = typeof(Reference<>).MakeGenericType(t);

            return jsonObject.ToObject(referenceType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
