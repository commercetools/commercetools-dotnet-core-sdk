using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ClientFixture
    {
        private ServiceProvider serviceProvider;
        private IConfiguration configuration;

        public ClientFixture()
        {
            var services = new ServiceCollection();
            this.configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json").
                AddEnvironmentVariables().
                Build();
            services.UseSerialization();
            services.UseHttpApiWithClientCredentials(configuration);
            serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.configuration.GetSection("Client").Get<ClientConfiguration>();
        }
    }
}
