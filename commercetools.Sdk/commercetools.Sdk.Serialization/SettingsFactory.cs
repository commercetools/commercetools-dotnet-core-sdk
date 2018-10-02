using commercetools.Sdk.HttpApi.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace commercetools.Sdk.Serialization
{
    public class JsonSerializerSettingsFactory
    {
        // TODO See if caching of settings is needed per type
        public static JsonSerializerSettings Create(Type type)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (type == typeof(Token))
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                settings.ContractResolver = contractResolver;
            }
            else
            {
                settings.ContractResolver = CustomContractResolver.Instance;
            }

            return settings;
        }
    }
}