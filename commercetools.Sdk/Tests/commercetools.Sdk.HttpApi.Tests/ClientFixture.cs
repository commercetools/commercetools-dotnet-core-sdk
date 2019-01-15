using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;

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
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddEnvironmentVariables().
                Build();
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            services.UseLinq();
            services.UseHttpApiWithClientCredentials(this.configuration, "Client");
            this.serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetServiceLocatorProvider(this.serviceProvider);
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

        // TODO Put this in a separate class
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}