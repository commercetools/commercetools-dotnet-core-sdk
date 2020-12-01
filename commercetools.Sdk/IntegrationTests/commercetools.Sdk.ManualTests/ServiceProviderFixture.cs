using System;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace commercetools.Sdk.ManualTests
{
    public class ServiceProviderFixture
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public ServiceProviderFixture(IMessageSink diagnosticMessageSink)
        {
            this.Configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                AddEnvironmentVariables("CTP_").
                Build();

            var services = new ServiceCollection();
            services.UseCommercetools(Configuration, "Client", TokenFlow.ClientCredentials);
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return this.ServiceProvider.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.Configuration.GetSection(name).Get<ClientConfiguration>();
        }
    }
}