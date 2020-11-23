using System.IO;
using commercetools.Sdk.Domain.Validation;
using commercetools.Sdk.HttpApi;
using Newtonsoft.Json.Serialization;

namespace commercetools.Sdk.HttpApi
{
    using Newtonsoft.Json;

    public class TokenSerializerService : ITokenSerializerService
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public TokenSerializerService()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };
        }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, jsonSerializerSettings);
        }
        
        public T Deserialize<T>(Stream input)
        {
            T result;
            using (var sr = new StreamReader(input))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.CreateDefault(jsonSerializerSettings);
                result = serializer.Deserialize<T>(reader);
            }

            return result;
        }

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input, jsonSerializerSettings);
        }
    }
}
