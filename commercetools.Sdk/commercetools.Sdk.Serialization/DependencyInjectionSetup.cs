namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.Util;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Reflection;

    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this IServiceCollection services)
        {
            services.RegisterAllInterfaceTypes(typeof(ICustomJsonMapper<>), ServiceLifetime.Singleton);
            services.RegisterAllInterfaceTypes(typeof(IMapperTypeRetriever<>), ServiceLifetime.Singleton);

            services.RegisterAllDerivedTypes<JsonConverterBase>(ServiceLifetime.Singleton);

            services.AddSingleton<IRegisteredTypeRetriever, RegisteredTypeRetriever>();
            services.RegisterAllInterfaceTypes(typeof(IDecoratorTypeRetriever<>), ServiceLifetime.Singleton);

            services.AddSingleton<DeserializationContractResolver>();
            services.AddSingleton<SerializationContractResolver>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
        }
    }
}