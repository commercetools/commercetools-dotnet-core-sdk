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

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input, jsonSerializerSettings);
        }
    }
}
