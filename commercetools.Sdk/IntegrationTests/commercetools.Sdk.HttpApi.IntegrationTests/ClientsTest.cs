using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ClientsTest
    {
        [Fact]
        public void MultipleClientsSimpleInjector()
        {
            var services = new Container();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddEnvironmentVariables().
                AddUserSecrets<ServiceProviderFixture>().
                Build();

            services.UseCommercetools(configuration,  new Dictionary<string, TokenFlow>()
            {
                { "Client", TokenFlow.ClientCredentials },
                { "TokenClient", TokenFlow.ClientCredentials }
            });
            var clients = services.GetServices<IClient>();
            Assert.IsAssignableFrom<IEnumerable<IClient>>(clients);
            var enumerable = clients.ToList();
            var client1 = enumerable.FirstOrDefault(client=> client.Name == "Client");
            var client2 = enumerable.FirstOrDefault(client => client.Name == "TokenClient");

            var result1 = client1.ExecuteAsync(new GetProjectCommand());
            var t1 = result1.Result;
            Assert.IsType<Sdk.Domain.Project.Project>(t1);
            var result2 = client2.ExecuteAsync(new GetProjectCommand());
            var t2 = result2.Result;
            Assert.IsType<Sdk.Domain.Project.Project>(t2);
        }

        [Fact]
        public void MultipleClientsBuiltIn()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddEnvironmentVariables().
                AddUserSecrets<ServiceProviderFixture>().
                Build();

            services.UseCommercetools(configuration,  new Dictionary<string, TokenFlow>()
            {
                { "Client", TokenFlow.ClientCredentials },
                { "TokenClient", TokenFlow.ClientCredentials }
            });
            var serviceProvider = services.BuildServiceProvider();
            var clients = serviceProvider.GetServices<IClient>();
            Assert.IsAssignableFrom<IEnumerable<IClient>>(clients);
            var enumerable = clients.ToList();
            var client1 = enumerable.FirstOrDefault(client=> client.Name == "Client");
            var client2 = enumerable.FirstOrDefault(client => client.Name == "TokenClient");

            var result1 = client1.ExecuteAsync(new GetProjectCommand());
            var t1 = result1.Result;
            Assert.IsType<Sdk.Domain.Project.Project>(t1);
            var result2 = client2.ExecuteAsync(new GetProjectCommand());
            var t2 = result2.Result;
            Assert.IsType<Sdk.Domain.Project.Project>(t2);
        }
    }
}
