using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace commercetools.Sdk.DependencyInjection
{
    /// <summary>
    /// Contains extensions methods to use when setting up commercetools dependencies.
    /// </summary>
    public static class DependencyInjectionSetup
    {
        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for a single client.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="tokenFlow">The token flow.</param>
        public static void UseCommercetools(this IServiceCollection services, IConfiguration configuration, string clientName = DefaultClientNames.Api, TokenFlow tokenFlow = TokenFlow.ClientCredentials)
        {
            services.UseCommercetools(configuration, new Dictionary<string, TokenFlow>() { { clientName, tokenFlow } });
        }

        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for multiple client.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clients">The clients with the client name as the key and the token flow as they value.</param>
        public static void UseCommercetools(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            services.UseLinq();
            services.UseHttpApi(configuration, clients);
            // TODO In case the service locator pattern is completely remove, remove these two lines as well
            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetServiceLocatorProvider(serviceProvider);
        }
    }
}
