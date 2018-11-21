using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class ErrorConverter : JsonConverter
    {
        private readonly IDecoratorTypeRetriever<Error> decoratorTypeRetriever;

        public ErrorConverter(IDecoratorTypeRetriever<Error> decoratorTypeRetriever)
        {
            this.decoratorTypeRetriever = decoratorTypeRetriever;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Error))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken codeProperty = jsonObject["code"];

            Type errorType = this.decoratorTypeRetriever.GetTypeForToken(codeProperty);

            // The default type is General Error in case a more specific one is not found.
            if (errorType == null)
            {
                errorType = typeof(GeneralError);
            }

            return jsonObject.ToObject(errorType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }       
    }
}