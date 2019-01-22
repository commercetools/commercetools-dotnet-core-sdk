using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.HttpApiCommands;
using commercetools.Sdk.HttpApi.RequestBuilders;
using commercetools.Sdk.HttpApi.SearchParameters;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.HttpApi.UploadImageParameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace commercetools.Sdk.HttpApi
{
    public static class DependencyInjectionSetup
    {
        public static void UseHttpApiWithClientCredentials(this IServiceCollection services, IConfiguration configuration, string configurationSectionName)
        {
            IClientConfiguration clientConfiguration = configuration.GetSection(configurationSectionName).Get<ClientConfiguration>();
            services.AddSingleton(clientConfiguration);
            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            services.AddSingleton(tokenFlowRegister);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();
            services.AddSingleton<AuthorizationHandler>();
            services.AddSingleton<CorrelationIdHandler>();
            services.AddSingleton<LoggerHandler>();

            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>().AddHttpMessageHandler<CorrelationIdHandler>().AddHttpMessageHandler<LoggerHandler>();

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

            services.AddSingleton<IClient, Client>();
        }

        public static void UseHttpApiWithPassword(this IServiceCollection services, IConfiguration configuration, string configurationSectionName)
        {
            IClientConfiguration clientConfiguration = configuration.GetSection(configurationSectionName).Get<ClientConfiguration>();
            services.AddSingleton(clientConfiguration);
            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<ITokenProvider, PasswordTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.Password;
            services.AddSingleton(tokenFlowRegister);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();
            services.AddSingleton<AuthorizationHandler>();
            services.AddSingleton<CorrelationIdHandler>();
            services.AddSingleton<LoggerHandler>();

            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>().AddHttpMessageHandler<CorrelationIdHandler>().AddHttpMessageHandler<LoggerHandler>();

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

            services.AddSingleton<IClient, Client>();
        }

        public static void UseHttpApiWithAnonymousSession(this IServiceCollection services, IConfiguration configuration, string configurationSectionName)
        {
            IClientConfiguration clientConfiguration = configuration.GetSection(configurationSectionName).Get<ClientConfiguration>();
            services.AddSingleton(clientConfiguration);
            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<ITokenProvider, AnonymousSessionTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.AnonymousSession;
            services.AddSingleton(tokenFlowRegister);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();
            services.AddSingleton<AuthorizationHandler>();
            services.AddSingleton<CorrelationIdHandler>();
            services.AddSingleton<LoggerHandler>();

            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>().AddHttpMessageHandler<CorrelationIdHandler>().AddHttpMessageHandler<LoggerHandler>();

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

            services.AddSingleton<IClient, Client>();
        }

        public static void UseHttpApi(this IServiceCollection services, IConfiguration configuration, string configurationSectionName)
        {
            IClientConfiguration clientConfiguration = configuration.GetSection(configurationSectionName).Get<ClientConfiguration>();
            services.AddSingleton(clientConfiguration);
            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>();
            services.AddSingleton<ITokenProvider, PasswordTokenProvider>();
            services.AddSingleton<ITokenProvider, AnonymousSessionTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.AnonymousSession;
            services.AddSingleton(tokenFlowRegister);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();
            services.AddSingleton<AuthorizationHandler>();
            services.AddSingleton<CorrelationIdHandler>();
            services.AddSingleton<LoggerHandler>();

            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>().AddHttpMessageHandler<CorrelationIdHandler>().AddHttpMessageHandler<LoggerHandler>();

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

            services.AddSingleton<IClient, Client>();
        }
    }
}