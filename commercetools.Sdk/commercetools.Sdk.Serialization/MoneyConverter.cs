using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class MoneyConverter : JsonConverter
    {
        private readonly IEnumerable<ICustomConverter<Money>> customConverters;

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Money))
            {
                return true;
            }
            return false;
        }

        public MoneyConverter(IEnumerable<ICustomConverter<Money>> customConverters)
        {
            this.customConverters = customConverters;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            Type moneyType;
            moneyType = GetTypeByValueProperty(jsonObject);
            if (moneyType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Money type cannot be determined.");
            }

            return jsonObject.ToObject(moneyType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private Type GetTypeByValueProperty(JToken valueProperty)
        {
            foreach (var customConvert in this.customConverters.OrderBy(c => c.Priority))
            {
                if (customConvert.CanConvert(valueProperty))
                {
                    return customConvert.Type;
                }
            }
            return null;
        }
    }
}