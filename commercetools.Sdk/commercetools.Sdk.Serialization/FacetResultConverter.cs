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
    // TODO Combine this with error converter
    public class FacetResultConverter : JsonConverter
    {
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

            Type errorType = GetTypeByCodeProperty(codeProperty);

            if (errorType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("FacetResult type cannot be determined.");
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
                                 where typeof(FacetResult).IsAssignableFrom(assemblyType)
                                 select assemblyType).ToArray();
            foreach (Type type in derivedErrors)
            {
                var attribute = type.CustomAttributes.Where(a => a.AttributeType == typeof(FacetTypeAttribute)).FirstOrDefault();
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
