using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                AddJsonFile("appsettings.test.Development.json", true).
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
            return this.configuration.GetSection(name).Get<ClientConfiguration>();
        }

        private static Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
