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
        }

        public ISerializerService SerializerService { get; private set; }
    }
}
