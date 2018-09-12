using commercetools.Sdk.HttpApi.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace commercetools.Sdk.Serialization
{
    public class JsonSerializerSettingsFactory
    {
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

            return settings;
        }
    }
}