using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Serialization.Tests
{
    public class SerializationFixture
    {
        public SerializationFixture()
        {
            var services = new ServiceCollection();
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            services.UseValidation();
            var serviceProvider = services.BuildServiceProvider();
            this.SerializerService = serviceProvider.GetService<ISerializerService>();

            var config = new SerializationConfiguration
            {
                DeserializeDateAttributesAsString = true,
                DeserializeDateTimeAttributesAsString = true
            };
            var servicesWithConfig = new ServiceCollection();
            servicesWithConfig.UseRegistration();
            servicesWithConfig.UseDomain();
            servicesWithConfig.UseSerialization();
            servicesWithConfig.UseValidation();
            servicesWithConfig.ConfigureSerializationServices(config);
            var serviceProviderWithConfig = servicesWithConfig.BuildServiceProvider();
            this.SerializerServiceWithDifferentConfig = serviceProviderWithConfig.GetService<ISerializerService>();
        }

        public ISerializerService SerializerService { get; private set; }
        
        public ISerializerService SerializerServiceWithDifferentConfig { get; private set; }
    }
}
