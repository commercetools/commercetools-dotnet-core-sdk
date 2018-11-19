using commercetools.Sdk.Client;
using commercetools.Sdk.Extensions;
using commercetools.Sdk.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public static class DependencyInjectionSetup
    {
        public static void UseHttpApiWithClientCredentials(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            IClientConfiguration clientConfiguration = configuration.GetSection("Client").Get<ClientConfiguration>();
            services.AddSingleton(clientConfiguration);
            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            services.AddSingleton(tokenFlowRegister);
            services.AddSingleton<AuthorizationHandler>();

            // TODO Add other handlers
            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>();

            services.AddSingleton<IQueryPredicateExpressionVisitor, QueryPredicateExpressionVisitor>();
            services.AddSingleton<IExpansionExpressionVisitor, ExpansionExpressionVisitor>();
            services.AddSingleton<ISortExpressionVisitor, SortExpressionVisitor>();
            services.AddSingleton<IFilterExpressionVisitor, FilterExpressionVisitor>();

            IRegisteredTypeRetriever registeredTypeRetriever = new RegisteredTypeRetriever(assembly);
            services.AddSingleton(registeredTypeRetriever);
            services.RegisterAllInterfaceTypes<IRequestMessageBuilder>(ServiceLifetime.Singleton, assembly);            

            services.AddSingleton<IHttpApiCommandFactory, HttpApiCommandFactory>();
            services.AddSingleton<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>();

            services.AddSingleton<IClient, Client>();
        }
    }
}
