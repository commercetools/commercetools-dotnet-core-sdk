using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                AddUserSecrets<ServiceProviderFixture>().
                AddEnvironmentVariables("CTP_").
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
            Assert.IsType<Project>(t1);
            var result2 = client2.ExecuteAsync(new GetProjectCommand());
            var t2 = result2.Result;
            Assert.IsType<Project>(t2);
        }

        [Fact]
        public void MultipleClientsBuiltIn()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddUserSecrets<ServiceProviderFixture>().
                AddEnvironmentVariables("CTP_").
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
            Assert.IsType<Project>(t1);
            var result2 = client2.ExecuteAsync(new GetProjectCommand());
            var t2 = result2.Result;
            Assert.IsType<Project>(t2);
        }

        [Fact]
        public async void TestConcurrentRequestsSimpleInject()
        {
            var services = new Container();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddUserSecrets<ServiceProviderFixture>().
                AddEnvironmentVariables("CTP_").
                Build();

            services.UseCommercetools(configuration, "Client");

            var serviceProvider = services;
            var client = serviceProvider.GetService<IClient>();

            var allTasks = new List<Task>();
            var throttler = new SemaphoreSlim(4);
            for (int i = 0; i < 500; i++)
            {
                await throttler.WaitAsync();
                allTasks.Add(
                    Task.Run(async () =>
                    {
                        try
                        {
                            var t1 = await client.ExecuteAsync(new GetProjectCommand());
                            Assert.IsType<Project>(t1);
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
            }

            await Task.WhenAll(allTasks);
        }

        [Fact]
        public async void TestConcurrentRequestsBuiltIn()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddUserSecrets<ServiceProviderFixture>().
                AddEnvironmentVariables("CTP_").
                Build();

            services.UseCommercetools(configuration, "Client");

            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetService<IClient>();

            var allTasks = new List<Task>();
            var throttler = new SemaphoreSlim(4);
            for (int i = 0; i < 500; i++)
            {
                await throttler.WaitAsync();
                allTasks.Add(
                    Task.Run(async () =>
                    {
                        try
                        {
                            var t1 = await client.ExecuteAsync(new GetProjectCommand());
                            Assert.IsType<Project>(t1);
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
            }

            await Task.WhenAll(allTasks);
        }
    }
}
