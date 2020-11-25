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
        }

        public static void ConfigureSerializationServices(
            this IServiceCollection serviceCollection,
            SerializationConfiguration serializationConfiguration)
        {
            if (serializationConfiguration.DeserializeDateAttributesAsString)
            {
                var serviceDescriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(DateAttributeMapper));
                serviceCollection.Remove(serviceDescriptor);
            }
            if (serializationConfiguration.DeserializeDateTimeAttributesAsString)
            {
                var serviceDescriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(DateTimeAttributeMapper));
                serviceCollection.Remove(serviceDescriptor);
            }
        }
    }
}