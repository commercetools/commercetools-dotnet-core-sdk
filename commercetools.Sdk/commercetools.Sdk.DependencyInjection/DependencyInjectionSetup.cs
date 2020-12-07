using System;
using System.Collections.Concurrent;
using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
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
        /// <param name="serializationConfiguration">The configuration of serialization services</param>
        public static IHttpClientBuilder UseCommercetools(this IServiceCollection services, IConfiguration configuration, string clientName = DefaultClientNames.Api, TokenFlow tokenFlow = TokenFlow.ClientCredentials, SerializationConfiguration serializationConfiguration = null)
        {
            var clients = new ConcurrentDictionary<string, TokenFlow>();
            clients.TryAdd(clientName, tokenFlow);
            return services.UseCommercetools(configuration, clients, serializationConfiguration).Single().Value;
        }

        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for multiple client.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clients">The clients with the client name as the key and the token flow as they value.</param>
        /// <param name="serializationConfiguration">The configuration of serialization services</param>
        public static IDictionary<string, IHttpClientBuilder> UseCommercetools(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients, SerializationConfiguration serializationConfiguration = null)
        {
            services.UseRegistration();
            services.UseLinq();
            services.UseDomain();
            services.UseSerialization(serializationConfiguration);
            return services.UseHttpApi(configuration, clients);
        }

        public static IDictionary<string, IHttpClientBuilder> ConfigureAllClients(this IDictionary<string, IHttpClientBuilder> httpClientBuilders,
            Action<IHttpClientBuilder> configureAction)
        {
            foreach (var clientBuilder in httpClientBuilders.Values) { configureAction(clientBuilder); }

            return httpClientBuilders;
        }

        public static IDictionary<string, IHttpClientBuilder> ConfigureClient(this IDictionary<string, IHttpClientBuilder> httpClientBuilders,
            string clientName, Action<IHttpClientBuilder> configureAction)
        {
            configureAction(httpClientBuilders[clientName]);
            return httpClientBuilders;
        }
    }
}
