using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Xml;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.DelegatingHandlers;
using commercetools.Sdk.HttpApi.HttpApiCommands;
using commercetools.Sdk.HttpApi.RequestBuilders;
using commercetools.Sdk.HttpApi.SearchParameters;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.HttpApi.UploadImageParameters;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace commercetools.Sdk.HttpApi
{
    public static class DependencyInjectionSetup
    {
        public static IHttpClientBuilder UseHttpApi(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            if (clients.Count() == 1)
            {
                return services.UseSingleClient(configuration, clients.First().Key, clients.First().Value);
            }

            services.UseMultipleClients(configuration, clients);
            return null;
        }

        private static void UseMultipleClients(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.UseHttpApiDefaults();
            TokenFlowMapper tokenFlowMapper = new TokenFlowMapper();
            foreach (KeyValuePair<string, TokenFlow> client in clients)
            {
                string clientName = client.Key;
                TokenFlow tokenFlow = client.Value;
                var clientConfigurationToRemove = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IClientConfiguration));
                services.Remove(clientConfigurationToRemove);
                var tokenFlowRegisterToRemove = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITokenFlowRegister));
                services.Remove(tokenFlowRegisterToRemove);
                services.SetupClient(configuration, clientName, tokenFlow);
                services.AddClient(clientName, tokenFlowMapper);
            }

            services.AddSingleton<ITokenFlowMapper>(tokenFlowMapper);
        }

        private static IHttpClientBuilder UseSingleClient(this IServiceCollection services, IConfiguration configuration, string clientName, TokenFlow tokenFlow)
        {
            services.UseHttpApiDefaults();
            IHttpClientBuilder httpClientBuilder = services.SetupClient(configuration, clientName, tokenFlow);
            services.AddSingleton<IClient>(c => new Client(c.GetService<IHttpClientFactory>(), c.GetService<IHttpApiCommandFactory>(), c.GetService<ISerializerService>()) { Name = clientName });
            return httpClientBuilder;
        }

        private static IHttpClientBuilder SetupClient(this IServiceCollection services, IConfiguration configuration, string clientName, TokenFlow tokenFlow)
        {
            IClientConfiguration clientConfiguration = configuration.GetSection(clientName).Get<ClientConfiguration>();
            Validator.ValidateObject(clientConfiguration, new ValidationContext(clientConfiguration), true);

            services.AddSingleton(clientConfiguration);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = tokenFlow;
            services.AddSingleton(tokenFlowRegister);
            IHttpClientBuilder httpClientBuilder = services.AddHttpClient(clientName)
                .AddHttpMessageHandler<AuthorizationHandler>().AddHttpMessageHandler<CorrelationIdHandler>()
                .AddHttpMessageHandler<LoggerHandler>()
                .AddHttpMessageHandler<ErrorHandler>();
            return httpClientBuilder;
        }

        private static void AddClient(this IServiceCollection services, string clientName, TokenFlowMapper tokenFlowMapper)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            tokenFlowMapper.Clients.Add(clientName, serviceProvider.GetService<ITokenFlowRegister>());
            IClient client = new Client(serviceProvider.GetService<IHttpClientFactory>(), serviceProvider.GetService<IHttpApiCommandFactory>(), serviceProvider.GetService<ISerializerService>()) { Name = clientName };
            services.AddSingleton(client);
        }

        private static void UseHttpApiDefaults(this IServiceCollection services)
        {
            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<IUserCredentialsStoreManager, InMemoryUserCredentialsStoreManager>();
            services.AddSingleton<IAnonymousCredentialsStoreManager, InMemoryAnonymousCredentialsStoreManager>();
            services.AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>();
            services.AddSingleton<ITokenProvider, PasswordTokenProvider>();
            services.AddSingleton<ITokenProvider, AnonymousSessionTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();
            services.AddSingleton<CorrelationIdHandler>();
            services.AddSingleton<LoggerHandler>();
            services.AddSingleton<ErrorHandler>();
            services.AddSingleton<AuthorizationHandler>();
            services.AddHttpClient(DefaultClientNames.Authorization);
            services.AddSingleton<IEndpointRetriever, EndpointRetriever>();
            services.RegisterAllTypes<IRequestMessageBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<IAdditionalParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<ISearchParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<IUploadImageParametersBuilder>(ServiceLifetime.Singleton);
            services.AddSingleton<IParametersBuilderFactory<IAdditionalParametersBuilder>, ParametersBuilderFactory<IAdditionalParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<ISearchParametersBuilder>, ParametersBuilderFactory<ISearchParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<IUploadImageParametersBuilder>, ParametersBuilderFactory<IUploadImageParametersBuilder>>();
            services.AddSingleton<IHttpApiCommandFactory, HttpApiCommandFactory>();
            services.AddSingleton<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>();
            services.AddSingleton<IApiExceptionFactory, ApiExceptionFactory>();
        }
    }
}
