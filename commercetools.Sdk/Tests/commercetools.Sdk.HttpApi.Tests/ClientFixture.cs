using commercetools.Sdk.Serialization;
using commercetools.Sdk.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ClientFixture
    {
        private readonly ServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public ClientFixture()
        {
            var services = new ServiceCollection();
            this.configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                AddEnvironmentVariables().
                Build();
            // TODO Combine this in all in one DI setup
            services.UseSerialization();
            services.UseHttpApiWithClientCredentials(this.configuration, "Client");
            this.serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(this.serviceProvider);
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