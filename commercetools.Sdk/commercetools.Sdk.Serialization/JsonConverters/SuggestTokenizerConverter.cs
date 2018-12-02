using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class SuggestTokenizerConverter : JsonConverterBase
    {
        private readonly IDecoratorTypeRetriever<SuggestTokenizer> decoratorTypeRetriever;

        public SuggestTokenizerConverter(IDecoratorTypeRetriever<SuggestTokenizer> decoratorTypeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(SuggestTokenizer))
            {
                return true;
            }
            return false;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken nameProperty = jsonObject["type"];

            Type suggestTokenizerType = this.decoratorTypeRetriever.GetTypeForToken(nameProperty);

            if (suggestTokenizerType == null)
            {
                throw new JsonSerializationException("Suggest tokenizer cannot be determined.");
            }

            return jsonObject.ToObject(suggestTokenizerType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}