using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.DelegatingHandlers;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Retry;
using Polly.Timeout;
using SimpleInjector;
using Xunit;

namespace commercetools.Sdk.IntegrationTests
{
    public class ClientsTest
    {
        [Fact]
        public async Task MultipleClientsSimpleInjector()
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

            var t1 = await client1.ExecuteAsync(new GetProjectCommand());
            Assert.IsType<Domain.Projects.Project>(t1);
            var t2 = await client2.ExecuteAsync(new GetProjectCommand());
            Assert.IsType<Domain.Projects.Project>(t2);
        }

        [Fact]
        public async Task MultipleClientsBuiltIn()
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

            var t1 = await client1.ExecuteAsync(new GetProjectCommand());
            Assert.IsType<Domain.Projects.Project>(t1);
            var t2 = await client2.ExecuteAsync(new GetProjectCommand());
            Assert.IsType<Domain.Projects.Project>(t2);
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
                            Assert.IsType<Domain.Projects.Project>(t1);
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
        public async void TestPreexistingHttpClientSimpleInject()
        {
            var services = new Container();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddUserSecrets<ServiceProviderFixture>().
                AddEnvironmentVariables("CTP_").
                Build();

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(ctx => TimeSpan.FromMilliseconds(0));

            var collection = new ServiceCollection();
            var builder = collection.AddHttpClient("Test", httpClient => httpClient.Timeout = TimeSpan.FromSeconds(20));

            builder.AddPolicyHandler(timeoutPolicy);

            services.RegisterSingleton(collection.BuildServiceProvider().GetService<IHttpClientFactory>());
            services.UseCommercetools(configuration, "Client");

            services.Verify();
            var clientFactory = services.GetService<IHttpClientFactory>();

            var client = services.GetService<IClient>();

            Assert.IsAssignableFrom<IClient>(client);

            var apiException = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await client.ExecuteAsync(new GetByIdCommand<Category>("notexisting")));
            Assert.True(apiException.Request.Headers.Contains("X-Correlation-Id"));

            var testClient = clientFactory.CreateClient("Test");
            Assert.IsType<HttpClient>(testClient);

            await Assert.ThrowsAsync<TimeoutRejectedException>(async () => await testClient.GetAsync("https://www.google.com"));
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
                            Assert.IsType<Domain.Projects.Project>(t1);
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
        public void TestCustomCorrelationIdProvider()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                AddUserSecrets<ServiceProviderFixture>().
                AddEnvironmentVariables("CTP_").
                Build();

            services.AddSingleton<ICorrelationIdProvider, CustomCorrelationIdProvider>();
            services.UseCommercetools(configuration, "Client");

            var serviceProvider = services.BuildServiceProvider();

            var correlationIdProviders = serviceProvider.GetServices<ICorrelationIdProvider>();
            Assert.NotNull(correlationIdProviders);
            Assert.IsType<CustomCorrelationIdProvider>(correlationIdProviders.FirstOrDefault());
        }
    }
}
