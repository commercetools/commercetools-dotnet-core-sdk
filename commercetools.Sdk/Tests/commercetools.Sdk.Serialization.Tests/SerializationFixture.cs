using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
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
            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetServiceLocatorProvider(serviceProvider);
            this.SerializerService = serviceProvider.GetService<ISerializerService>();
        }

        public ISerializerService SerializerService { get; private set; }
    }
}