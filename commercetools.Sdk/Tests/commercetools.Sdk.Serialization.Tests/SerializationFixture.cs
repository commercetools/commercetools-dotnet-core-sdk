using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Serialization.Tests
{
    public class SerializationFixture
    {
        public SerializationFixture()
        {
            var services = new ServiceCollection();
            services.UseUtil();
            services.UseDomain();
            services.UseSerialization();
            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(serviceProvider);
            this.SerializerService = serviceProvider.GetService<ISerializerService>();
        }

        public ISerializerService SerializerService { get; private set; }
    }
}