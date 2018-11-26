using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FacetResultConverter : JsonConverterBase
    {
        private readonly IDecoratorTypeRetriever<FacetResult> decoratorTypeRetriever;

        public FacetResultConverter(IDecoratorTypeRetriever<FacetResult> decoratorTypeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(FacetResult))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken codeProperty = jsonObject["type"];

            Type facetResultType = this.decoratorTypeRetriever.GetTypeForToken(codeProperty);

            if (facetResultType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("FacetResult type cannot be determined.");
            }

            return jsonObject.ToObject(facetResultType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
