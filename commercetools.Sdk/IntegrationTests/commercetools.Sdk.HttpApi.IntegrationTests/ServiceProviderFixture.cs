using System;
using commercetools.Sdk.DependencyInjection;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.SimpleInjector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ServiceProviderFixture
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public ServiceProviderFixture(IMessageSink diagnosticMessageSink)
        {
            //services.AddLogging(configure => configure.AddConsole());
            this.Configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddEnvironmentVariables().
                AddUserSecrets<ServiceProviderFixture>().
                Build();

            var containerType = Enum.Parse<ContainerType>(Configuration.GetValue("Container", "BuiltIn"));
            diagnosticMessageSink.OnMessage(new DiagnosticMessage("Use container {0}", containerType.ToString()));
            switch (containerType)
            {
                case ContainerType.BuiltIn:
                    var services = new ServiceCollection();
                    services.UseCommercetools(Configuration, "Client", TokenFlow.ClientCredentials);
                    this.ServiceProvider = services.BuildServiceProvider();
                    break;
                case ContainerType.SimpleInjector:
                    var container = new Container();
                    container.UseCommercetools(Configuration, "Client", TokenFlow.ClientCredentials);
                    this.ServiceProvider = container;
                    break;
            }
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
