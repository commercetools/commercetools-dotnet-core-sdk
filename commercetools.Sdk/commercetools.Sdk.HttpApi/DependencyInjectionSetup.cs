using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.HttpApiCommands;
using commercetools.Sdk.HttpApi.RequestBuilders;
using commercetools.Sdk.HttpApi.SearchParameters;
using commercetools.Sdk.HttpApi.UploadImageParameters;
using commercetools.Sdk.Linq;
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
            services.AddSingleton<IQueryPredicateExpressionVisitor, QueryPredicateExpressionVisitor>();
            services.AddSingleton<IExpansionExpressionVisitor, ExpansionExpressionVisitor>();
            services.AddSingleton<ISortExpressionVisitor, SortExpressionVisitor>();
            services.AddSingleton<IFilterExpressionVisitor, FilterExpressionVisitor>();

            services.RegisterAllInterfaceTypes<IRequestMessageBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllInterfaceTypes<IAdditionalParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllInterfaceTypes<ISearchParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllInterfaceTypes<IUploadImageParametersBuilder>(ServiceLifetime.Singleton);

            services.AddSingleton<IParametersBuilderFactory<IAdditionalParametersBuilder>, ParametersBuilderFactory<IAdditionalParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<ISearchParametersBuilder>, ParametersBuilderFactory<ISearchParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<IUploadImageParametersBuilder>, ParametersBuilderFactory<IUploadImageParametersBuilder>>();

            services.AddSingleton<IHttpApiCommandFactory, HttpApiCommandFactory>();
            services.AddSingleton<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>();

            services.AddSingleton<IClient, Client>();
        }
    }
}