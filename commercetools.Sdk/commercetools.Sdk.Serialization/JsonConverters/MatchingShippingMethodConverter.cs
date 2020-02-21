using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public class MatchingShippingMethodConverter: JsonConverterBase
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(PagedQueryResult<ShippingMethod>))
            {
                return true;
            }
            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jsonToken = JToken.Load(reader);
            var result = new PagedQueryResult<ShippingMethod>();
            if (jsonToken is JObject jObject)
            {
                foreach (var property in jObject)
                {
                    switch (property.Key)
                    {
                        case "limit": result.Limit = property.Value.ToObject<int>(serializer); break;
                        case "offset": result.Offset = property.Value.ToObject<int>(serializer); break;
                        case "total": result.Total = property.Value.ToObject<int>(serializer); break;
                        case "count": result.Count = property.Value.ToObject<int>(serializer); break;
                        case "results": result.Results = property.Value.ToObject<List<ShippingMethod>>(serializer); break;
                    }
                }
            }
            else if (jsonToken is JArray jArray)
            {
                result.Count = jArray.Count;
                result.Results = jArray.ToObject<List<ShippingMethod>>(serializer);
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };
    }
}