using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class MoneyConverter : JsonConverterBase
    {
        private readonly IMapperTypeRetriever<BaseMoney> mapperTypeRetriever;

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(BaseMoney))
            {
                return true;
            }
            return false;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public MoneyConverter(IMapperTypeRetriever<BaseMoney> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            Type moneyType = this.mapperTypeRetriever.GetTypeForToken(jsonObject);

            if (moneyType == null)
            {
                throw new JsonSerializationException();
            }

            return jsonObject.ToObject(moneyType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}