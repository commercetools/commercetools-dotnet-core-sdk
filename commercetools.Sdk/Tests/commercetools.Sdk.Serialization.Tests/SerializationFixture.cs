using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Serialization.Tests
{
    public class SerializationFixture
    {
        public SerializationFixture()
        {
            var services = new ServiceCollection();
            services.UseSerialization();
            var serviceProvider = services.BuildServiceProvider();
            SerializerService = serviceProvider.GetService<ISerializerService>();
        }

        public ISerializerService SerializerService { get; private set; }
    }
}