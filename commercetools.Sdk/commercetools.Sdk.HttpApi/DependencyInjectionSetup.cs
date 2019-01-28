using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
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
        public static void UseHttpApi(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.UseHttpApiDefaults();
            TokenFlowMapper tokenFlowMapper = new TokenFlowMapper();
            foreach (KeyValuePair<string, TokenFlow> client in clients)
            {
                services.AddClient(configuration, client.Key, client.Value, tokenFlowMapper);
            }

            services.AddSingleton<ITokenFlowMapper>(tokenFlowMapper);
        }

        private static void AddClient(this IServiceCollection services, IConfiguration configuration, string clientName, TokenFlow tokenFlow, TokenFlowMapper tokenFlowMapper)
        {
            var clientConfigurationToRemove = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IClientConfiguration));
            services.Remove(clientConfigurationToRemove);
            IClientConfiguration clientConfiguration = configuration.GetSection(clientName).Get<ClientConfiguration>();
            services.AddSingleton(clientConfiguration);
            var tokenFlowRegisterToRemove = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITokenFlowRegister));
            services.Remove(tokenFlowRegisterToRemove);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = tokenFlow;
            services.AddSingleton(tokenFlowRegister);
            tokenFlowMapper.Clients.Add(clientName, tokenFlowRegister);
            services.AddHttpClient(clientName).AddHttpMessageHandler<AuthorizationHandler>().AddHttpMessageHandler<CorrelationIdHandler>().AddHttpMessageHandler<LoggerHandler>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
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
        }
    }
}