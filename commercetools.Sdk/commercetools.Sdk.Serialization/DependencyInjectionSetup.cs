namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.Util;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this IServiceCollection services)
        {
            services.RegisterAllInterfaceTypes(typeof(ICustomJsonMapper<>), ServiceLifetime.Singleton);
            services.RegisterAllInterfaceTypes(typeof(IMapperTypeRetriever<>), ServiceLifetime.Singleton);

            services.RegisterAllDerivedTypes<JsonConverterBase>(ServiceLifetime.Singleton);

            services.RegisterAllInterfaceTypes(typeof(IDecoratorTypeRetriever<>), ServiceLifetime.Singleton);

            services.AddSingleton<DeserializationContractResolver>();
            services.AddSingleton<SerializationContractResolver>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
        }
    }
}