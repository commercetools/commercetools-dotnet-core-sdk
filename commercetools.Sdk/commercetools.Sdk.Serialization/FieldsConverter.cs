using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FieldsConverter : JsonConverter
    {
        private readonly IMapperTypeRetriever<Fields> typeRetriever;

        public FieldsConverter(IMapperTypeRetriever<Fields> typeRetriever)
        {
            this.typeRetriever = typeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Fields))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Fields customFields = new Fields();
            JObject jsonObject = JObject.Load(reader);
            if (jsonObject.Count == 0)
            {
                return customFields;
            }

            foreach (JProperty property in jsonObject.Children())
            {
                string key = property.Name;
                JToken value = property.Value;
                Type valueType = this.typeRetriever.GetTypeForToken(value);
                object o = value.ToObject(valueType, serializer);
                customFields.Add(key, o);
            }

            return customFields;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
