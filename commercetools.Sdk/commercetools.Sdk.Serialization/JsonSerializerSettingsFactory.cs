namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.HttpApi.Domain;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;

    public class JsonSerializerSettingsFactory
    {
        private readonly DeserializationContractResolver deserializationContractResolver;
        private readonly SerializationContractResolver serializationContractResolver;
        private IDictionary<Type, JsonSerializerSettings> mapping = new Dictionary<Type, JsonSerializerSettings>();

        public JsonSerializerSettingsFactory(DeserializationContractResolver deserializationContractResolver, SerializationContractResolver serializationContractResolver)
        {
            this.deserializationContractResolver = deserializationContractResolver;
            this.serializationContractResolver = serializationContractResolver;
        }

        public JsonSerializerSettings CreateDeserializationSettings(Type type)
        {
            if (!mapping.ContainsKey(type))
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
                    settings.ContractResolver = this.deserializationContractResolver;
                }

                mapping[type] = settings;
            }

            return mapping[type];
        }

        public JsonSerializerSettings CreateSerializationSettings(Type type)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            settings.ContractResolver = this.serializationContractResolver;
            return settings;
        }
    }
}
