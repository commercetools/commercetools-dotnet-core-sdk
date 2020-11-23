using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using Newtonsoft.Json;

namespace commercetools.Sdk.Serialization
{
    public class JsonSerializerSettingsFactory
    {
        private readonly DeserializationContractResolver deserializationContractResolver;
        private readonly SerializationContractResolver serializationContractResolver;
        private ConcurrentDictionary<Type, JsonSerializerSettings> mapping = new ConcurrentDictionary<Type, JsonSerializerSettings>();

        public JsonSerializerSettingsFactory(DeserializationContractResolver deserializationContractResolver, SerializationContractResolver serializationContractResolver)
        {
            this.deserializationContractResolver = deserializationContractResolver;
            this.serializationContractResolver = serializationContractResolver;
        }

        public JsonSerializerSettings CreateDeserializationSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = this.deserializationContractResolver;
            settings.DateParseHandling = DateParseHandling.None;
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            return settings;
        }
        
        public JsonSerializerSettings CreateDeserializationSettings(Type type)
        {
            try
            {
                return mapping.GetOrAdd(type, obj => CreateDeserializationSettings());
            }
            catch (NullReferenceException)
            {
                return null;//default serialization settings will be used
            }
        }

        public JsonSerializerSettings CreateSerializationSettings(Type type)
        {
            return CreateDeserializationSettings();
        }

        public JsonSerializerSettings CreateSerializationSettings()
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
