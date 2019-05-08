using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Project;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using Xunit;

namespace commercetools.Sdk.SimpleInjector.Tests
{
    public class SimpleInjectorSetupTest
    {
        [Fact]
        public void UseCommercetools()
        {
            var container = new Container();

            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                AddEnvironmentVariables().
                AddUserSecrets<SimpleInjectorSetupTest>().
                Build();

            container.UseCommercetools(configuration, "Client", TokenFlow.ClientCredentials);

            var client = container.GetInstance<IClient>();

            Assert.IsAssignableFrom<IClient>(client);

            var result = client.ExecuteAsync(new GetProjectCommand());

            var project = result.Result;
            Assert.NotNull(project);
        }
    }
}
