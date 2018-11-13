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
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(HttpApi.Domain.Error))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken codeProperty = jsonObject["code"];
            
            Type errorType = GetTypeByCodeProperty(codeProperty);

            // The default type is General Error in case a more specific one is found
            if (errorType == null)
            {
                errorType = typeof(HttpApi.Domain.GeneralError);
            }

            return jsonObject.ToObject(errorType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        

        

        private Type GetTypeByCodeProperty(JToken codeProperty)
        {
            // TODO Take this out of here and inject
            var derivedErrors = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from assemblyType in domainAssembly.GetTypes()
                            where typeof(HttpApi.Domain.Error).IsAssignableFrom(assemblyType)
                            select assemblyType).ToArray();
            foreach(Type type in derivedErrors)
            {
                var attribute = type.CustomAttributes.Where(a => a.AttributeType == typeof(StatusCodeAttribute)).FirstOrDefault();
                if (attribute != null)
                {
                    // add null checks
                    if (attribute.ConstructorArguments[0].Value.ToString() == codeProperty.Value<string>())
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        
    }
}