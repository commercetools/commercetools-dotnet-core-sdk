using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class MoneyConverter : JsonConverterBase
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

            Type moneyType;
            if (jsonObject.ContainsKey("type")) {
                switch (jsonObject.GetValue("type").Value<string>())
                {
                    case "highPrecision": moneyType = typeof(HighPrecisionMoney); break;  
                    case "centPrecision": moneyType = typeof(Money); break;
                    default: moneyType = this.mapperTypeRetriever.GetTypeForToken(jsonObject); break;
                }
            } else {
                moneyType = this.mapperTypeRetriever.GetTypeForToken(jsonObject);
            }

            if (moneyType == null)
            {
                throw new JsonSerializationException($"Unknown money type: {jsonObject.ToString(Formatting.None)}");
            }

            return jsonObject.ToObject(moneyType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
