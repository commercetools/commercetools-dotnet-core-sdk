using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using Xunit;

namespace commercetools.Sdk.DI.SI.Tests
{
    public class DependencyInjectionSetupTests
    {
        [Fact]
        public void SimpleInjectorContainerHasAllDependenciesRegistered()
        {
            // Arrange
            var container = new Container();
            var memoryFileProvider = new InMemoryFileProvider("{ \"Client\": { \"ClientId\": \"\", \"ClientSecret\": \"\", \"Scope\": \"manage_project:portablevendor\", \"ProjectKey\": \"portablevendor\" } }");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(memoryFileProvider, "appsettings.json", false, false)
                .Build();

            // Act
            container.UseCommercetools(configuration, "Client", TokenFlow.AnonymousSession);
            var client = container.GetInstance<Client.IClient>();

            // Assert
            container.Verify();
            Assert.NotNull(client);
        }
    }
}
