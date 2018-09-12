using commercetools.Sdk.HttpApi.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace commercetools.Sdk.Serialization
{
    public class SettingsFactory
    {
        public static JsonSerializerSettings Create(Type obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (obj == typeof(Token))
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                settings.ContractResolver = contractResolver;
            }

            return settings;
        }
    }
}