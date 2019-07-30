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
            if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(SuggestionResult<>))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var suggestionType = objectType.GenericTypeArguments[0];
            var listType = typeof(List<>).MakeGenericType(suggestionType);
            var resultType = typeof(SuggestionResult<>).MakeGenericType(suggestionType);
            var result = Activator.CreateInstance(resultType);

            JObject jsonObject = JObject.Load(reader);
            if (jsonObject != null)
            {
                foreach (var jToken in (JToken) jsonObject)
                {
                    var property = (JProperty) jToken;
                    string name = property.Name.Replace(ConstSearchKeywordsText, "");
                    JToken value = property.Value;

                    var suggestions = serializer.Deserialize(value.CreateReader(), listType);
                    var addSuggestionsMethod = result.GetType().GetMethod("AddSuggestions");
                    if (addSuggestionsMethod != null)
                        addSuggestionsMethod.Invoke(result, new object[] {name, suggestions});
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
