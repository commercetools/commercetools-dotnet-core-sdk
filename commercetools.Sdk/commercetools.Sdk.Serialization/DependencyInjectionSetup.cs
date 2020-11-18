using commercetools.Sdk.Domain.Validation;
using commercetools.Sdk.Serialization.JsonConverters;

namespace commercetools.Sdk.Serialization
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this IServiceCollection services, AttributeReaderMode mode = AttributeReaderMode.Standard)
        {
            services.RegisterAllTypes(typeof(ICustomJsonMapper<>), ServiceLifetime.Singleton);
            services.RegisterAllTypes(typeof(IMapperTypeRetriever<>), ServiceLifetime.Singleton);

            services.RegisterAllTypes<JsonConverterBase>(ServiceLifetime.Singleton);

            services.RegisterAllTypes(typeof(IDecoratorTypeRetriever<>), ServiceLifetime.Singleton);
            switch (mode)
            {
                case AttributeReaderMode.Cached: 
                    services.AddSingleton<IAttributeConverterReader, CachedAttributeConverterReader>();
                    services.AddSingleton<IFieldConverterReader, CachedFieldConverterReader>();
                    break;
                case AttributeReaderMode.Simple:
                    services.AddSingleton<IAttributeConverterReader, SimpleAttributeConverterReader>();
                    services.AddSingleton<IFieldConverterReader, SimpleFieldConverterReader>();
                    break;
                default:
                    services.AddSingleton<IAttributeConverterReader, StandardAttributeConverterReader>();
                    services.AddSingleton<IFieldConverterReader, StandardFieldConverterReader>();
                    break;
            }
            
            services.AddSingleton<DeserializationContractResolver>();
            services.AddSingleton<SerializationContractResolver>();
            services.AddSingleton<IModelValidator, NullModelValidator>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
        }
    }
}
