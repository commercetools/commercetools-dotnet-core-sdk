using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;
using commercetools.Sdk.Domain.Suggestions;

namespace commercetools.Sdk.Serialization
{
    internal class SuggestionResultConverter : JsonConverterBase
    {
        private const string ConstSearchKeywordsText = "searchKeywords.";
        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(SuggestionResult<ProductSuggestion>))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new SuggestionResult<ProductSuggestion>();
            JObject jsonObject = JObject.Load(reader);
            if (jsonObject != null)
            {
                foreach (var jToken in (JToken) jsonObject)
                {
                    var property = (JProperty) jToken;
                    string name = property.Name.Replace(ConstSearchKeywordsText, "");
                    JToken value = property.Value;

                    var suggestions = serializer.Deserialize<List<ProductSuggestion>>(value.CreateReader());
                    result.Suggestions.Add(name, suggestions);
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
