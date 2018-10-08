namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.HttpApi.Domain;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;

    public class JsonSerializerSettingsFactory
    {
        private CustomContractResolver customContractResolver;
        private IDictionary<Type, JsonSerializerSettings> mapping = new Dictionary<Type, JsonSerializerSettings>();

        public JsonSerializerSettingsFactory(CustomContractResolver customContractResolver)
        {
            this.customContractResolver = customContractResolver;
        }

        public JsonSerializerSettings Create(Type type)
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
                    settings.ContractResolver = this.customContractResolver;
                }

                mapping[type] = settings;
            }

            return mapping[type];
        }
    }
}