using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public class StandardFieldConverterReader : IFieldConverterReader
    {
        private readonly IMapperTypeRetriever<Fields> mapperTypeRetriever;
        
        public StandardFieldConverterReader(IMapperTypeRetriever<Fields> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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
                Type valueType = this.mapperTypeRetriever.GetTypeForToken(value);
                object o = value.ToObject(valueType, serializer);
                customFields.Add(key, o);
            }

            return customFields;
        }
    }
}