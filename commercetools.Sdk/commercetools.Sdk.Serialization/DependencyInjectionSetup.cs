using commercetools.Sdk.Domain;
using commercetools.Sdk.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // TODO Do not specify T 
            services.RegisterAllInterfaceTypes<ICustomJsonMapper<Domain.Attribute>>(ServiceLifetime.Singleton, assembly);
            services.RegisterAllInterfaceTypes<ICustomJsonMapper<Fields>>(ServiceLifetime.Singleton, assembly);
            services.RegisterAllInterfaceTypes<ICustomJsonMapper<Money>>(ServiceLifetime.Singleton, assembly);
            
            // TODO Do not specify T
            services.AddSingleton<IMapperTypeRetriever<Domain.Attribute>, SetAttributeMapperTypeRetriever>();
            services.AddSingleton<IMapperTypeRetriever<Fields>, SetFieldMapperTypeRetriever>();
            services.AddSingleton<IMapperTypeRetriever<Money>, MapperTypeRetriever<Money>>();

            services.RegisterAllDerivedTypes<JsonConverter>(ServiceLifetime.Singleton, assembly);

            services.AddSingleton<CustomContractResolver>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
        }
    }
}
