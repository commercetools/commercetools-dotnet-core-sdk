using System.Collections.Generic;
using commercetools.Sdk.DependencyInjection.SimpleInjector;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector.Lifestyles;

namespace SimpleInjector
{
    /// <summary>
    /// Contains extensions methods to use when setting up commercetools dependencies.
    /// </summary>
    public static class DependencyInjectionSetup
    {
        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for a single client.
        /// </summary>
        /// <param name="container">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="tokenFlow">The token flow.</param>
        public static void UseCommercetools(this Container container, IConfiguration configuration, string clientName = DefaultClientNames.Api, TokenFlow tokenFlow = TokenFlow.ClientCredentials)
        {
            container.UseCommercetools(configuration, new Dictionary<string, TokenFlow>() { { clientName, tokenFlow } });
        }

        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for multiple client.
        /// </summary>
        /// <param name="container">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clients">The clients with the client name as the key and the token flow as they value.</param>
        public static void UseCommercetools(this Container container, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            IServiceCollection services = new ServiceCollection();

            services.UseCommercetools(configuration, clients);

            if (container.Options.DefaultScopedLifestyle == null)
            {
                container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            }

            services.EnableSimpleInjectorCrossWiring(container);
            container.AutoCrossWireAspNetComponents(new ApplicationServicesWrapper { ApplicationServices = services.BuildServiceProvider() });
        }
    }
}