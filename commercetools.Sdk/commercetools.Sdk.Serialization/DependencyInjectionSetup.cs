namespace commercetools.Sdk.Serialization
{
    using commercetools.Sdk.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System.Reflection;

    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this IServiceCollection services)
        {
            services.RegisterAllInterfaceTypes(typeof(ICustomJsonMapper<>), ServiceLifetime.Singleton);
            services.RegisterAllInterfaceTypes(typeof(IMapperTypeRetriever<>), ServiceLifetime.Singleton);

            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllDerivedTypes<JsonConverter>(ServiceLifetime.Singleton, assembly);

            services.AddSingleton<CustomContractResolver>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
        }
    }
}