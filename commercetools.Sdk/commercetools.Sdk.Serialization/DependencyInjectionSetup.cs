using System.Linq;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this IServiceCollection services)
        {
            services.RegisterAllTypes(typeof(ICustomJsonMapper<>), ServiceLifetime.Singleton);
            services.RegisterAllTypes(typeof(IMapperTypeRetriever<>), ServiceLifetime.Singleton);

            services.RegisterAllTypes<JsonConverterBase>(ServiceLifetime.Singleton);

            services.RegisterAllTypes(typeof(IDecoratorTypeRetriever<>), ServiceLifetime.Singleton);

            services.AddSingleton<DeserializationContractResolver>();
            services.AddSingleton<SerializationContractResolver>();
            services.AddSingleton<IModelValidator, NullModelValidator>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
            services.AddSingleton<ISerializationConfiguration, SerializationConfiguration>();
        }

        public static void ConfigureSerializationServices(
            this IServiceCollection services,
            ISerializationConfiguration serializationConfiguration)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ISerializationConfiguration));
            if (serviceDescriptor != null)
                services.Remove(serviceDescriptor);
            services.AddSingleton(serializationConfiguration);
        }
    }
}