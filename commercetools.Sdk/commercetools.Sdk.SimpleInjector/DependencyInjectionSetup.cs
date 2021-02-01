using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Validation;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.DelegatingHandlers;
using commercetools.Sdk.HttpApi.HttpApiCommands;
using commercetools.Sdk.HttpApi.RequestBuilders;
using commercetools.Sdk.HttpApi.SearchParameters;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.HttpApi.UploadImageParameters;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Linq.Filter;
using commercetools.Sdk.Linq.Query;
using commercetools.Sdk.Linq.Sort;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Serialization;
using commercetools.Sdk.Serialization.JsonConverters;
using commercetools.Sdk.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using CultureValidator = commercetools.Sdk.Domain.Validation.CultureValidator;

namespace SimpleInjector
{
    public static class DependencyInjectionSetup
    {
        public static void UseSerialization(this Container services)
        {
            services.RegisterCollection(typeof(ICustomJsonMapper<>), typeof(ICustomJsonMapper<>).Assembly);
            services.Register(typeof(IMapperTypeRetriever<>), new [] {typeof(IMapperTypeRetriever<>).Assembly});

            services.RegisterCollection(typeof(JsonConverterBase), typeof(JsonConverterBase).Assembly);

            services.Register(typeof(IDecoratorTypeRetriever<>), new [] {typeof(IMapperTypeRetriever<>).Assembly});

            services.Register<DeserializationContractResolver>(Lifestyle.Singleton);
            services.Register<SerializationContractResolver>(Lifestyle.Singleton);
            services.Register<IModelValidator, NullModelValidator>(Lifestyle.Singleton);
            services.Register<JsonSerializerSettingsFactory>(Lifestyle.Singleton);
            services.Register<ISerializerService, SerializerService>(Lifestyle.Singleton);
            services.Register<ISerializationConfiguration, SerializationConfiguration>(Lifestyle.Singleton);
        }

        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void UseRegistration(this Container services)
        {
            services.Register<ITypeRetriever, TypeRetriever>(Lifestyle.Singleton);
        }

        public static void UseValidation(this Container services)
        {
            services.Options.AllowOverridingRegistrations = true;
            services.RegisterCollection(typeof(IResourceValidator), typeof(IResourceValidator).Assembly);
            services.Register<IModelValidator, ModelValidator>(Lifestyle.Singleton);
        }

        public static void UseLinq(this Container services)
        {
            services.Register<IQueryPredicateExpressionVisitor, QueryPredicateExpressionVisitor>(Lifestyle.Singleton);
            services.Register<IFilterPredicateExpressionVisitor, FilterPredicateExpressionVisitor>(Lifestyle.Singleton);
            services.Register<IDiscountPredicateExpressionVisitor, DiscountPredicateExpressionVisitor>(Lifestyle.Singleton);

            services.Register<IExpansionExpressionVisitor, ExpansionExpressionVisitor>(Lifestyle.Singleton);
            services.Register<ISortPredicateExpressionVisitor, SortPredicateExpressionVisitor>(Lifestyle.Singleton);
        }

        public static void UseDomain(this Container services)
        {
            services.Register<ICultureValidator, CultureValidator>(Lifestyle.Singleton);

        }

        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for a single client.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="tokenFlow">The token flow.</param>
        public static IHttpClientBuilder UseCommercetools(this Container services, IConfiguration configuration,
            string clientName = DefaultClientNames.Api, TokenFlow tokenFlow = TokenFlow.ClientCredentials)
        {
            var clients = new ConcurrentDictionary<string, TokenFlow>();
            clients.TryAdd(clientName, tokenFlow);
            return services
                .UseCommercetools(configuration, clients).Single()
                .Value;
        }

        /// <summary>
        /// Adds concrete implementations necessary for running of the application to the service collection for multiple client.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="clients">The clients with the client name as the key and the token flow as they value.</param>
        public static IDictionary<string, IHttpClientBuilder> UseCommercetools(this Container services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.UseRegistration();
            services.UseLinq();
            services.UseDomain();
            services.UseSerialization();
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

        public static IDictionary<string, IHttpClientBuilder> UseHttpApi(this Container services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {

            if (clients.Count() == 1)
            {
                return services.UseSingleClient(configuration, clients.First().Key, clients.First().Value);
            }

            return services.UseMultipleClients(configuration, clients);

        }

        private static IDictionary<string, IHttpClientBuilder> UseMultipleClients(this Container services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            var collection = new ServiceCollection();
            services.UseHttpApiDefaults();

            var builders = new ConcurrentDictionary<string, IHttpClientBuilder>();
            var clientBuilders = new List<Registration>();
            foreach (KeyValuePair<string, TokenFlow> client in clients)
            {
                string clientName = client.Key;
                TokenFlow tokenFlow = client.Value;

                IClientConfiguration clientConfiguration = configuration.GetSection(clientName).Get<ClientConfiguration>();
                Validator.ValidateObject(clientConfiguration, new ValidationContext(clientConfiguration), true);

                builders.TryAdd(clientName, services.SetupClient(collection, clientName, clientConfiguration, tokenFlow));

                clientBuilders.Add(Lifestyle.Singleton.CreateRegistration(() => new CtpClient(collection.BuildServiceProvider().GetService<IHttpClientFactory>(), services.GetService<IHttpApiCommandFactory>(), services.GetService<ISerializerService>(), services.GetService<IUserAgentProvider>()) { Name = clientName }, services));
            }
            services.RegisterCollection<IClient>(clientBuilders);

            collection.AddHttpClient(DefaultClientNames.Authorization);
            services.UseTokenProviders(collection.BuildServiceProvider().GetService<IHttpClientFactory>());

            return builders;
        }

        private static void UseTokenProviders(this Container services, IHttpClientFactory factory)
        {
            var providers = new List<Registration>
            {
                Lifestyle.Transient.CreateRegistration(
                    () => new AnonymousSessionTokenProvider(factory,
                        services.GetService<IAnonymousCredentialsStoreManager>(),
                        services.GetService<ITokenSerializerService>()), services),
                Lifestyle.Transient.CreateRegistration(
                    () => new ClientCredentialsTokenProvider(factory, services.GetService<ITokenStoreManager>(),
                        services.GetService<ITokenSerializerService>()), services),
                Lifestyle.Transient.CreateRegistration(
                    () => new PasswordTokenProvider(factory, services.GetService<IUserCredentialsStoreManager>(),
                        services.GetService<ITokenSerializerService>()), services)
            };

            services.RegisterCollection<ITokenProvider>(providers);
        }

        private static IDictionary<string, IHttpClientBuilder> UseSingleClient(this Container services, IConfiguration configuration, string clientName, TokenFlow tokenFlow)
        {
            var collection = new ServiceCollection();
            services.UseHttpApiDefaults();

            var configurationSection = configuration.GetSection(clientName);
            IClientConfiguration clientConfiguration = configurationSection.Get<ClientConfiguration>();
            Validator.ValidateObject(clientConfiguration, new ValidationContext(clientConfiguration), true);

            var builders = new ConcurrentDictionary<string, IHttpClientBuilder>();
            builders.TryAdd(clientName, services.SetupClient(collection, clientName, clientConfiguration, tokenFlow));
            
            services.Register<IClient>(() => new CtpClient(collection.BuildServiceProvider().GetService<IHttpClientFactory>(), services.GetService<IHttpApiCommandFactory>(), services.GetService<ISerializerService>(), services.GetService<IUserAgentProvider>()) { Name = clientName }, Lifestyle.Singleton);

            collection.AddHttpClient(DefaultClientNames.Authorization);
            services.UseTokenProviders(collection.BuildServiceProvider().GetService<IHttpClientFactory>());

            return builders;
        }

        private static IHttpClientBuilder SetupClient(this Container services, IServiceCollection collection, string clientName, IClientConfiguration clientConfiguration, TokenFlow tokenFlow)
        {
            var httpClientBuilder = collection.AddHttpClient(clientName)
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(clientConfiguration.ApiBaseAddress + clientConfiguration.ProjectKey + "/");
                    client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
                    client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("deflate");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(services.GetService<IUserAgentProvider>().UserAgent);
                })
                .ConfigureHttpMessageHandlerBuilder(builder =>
                {
                    builder.PrimaryHandler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                    };
                })
                .AddHttpMessageHandler(c =>
                {
                    var providers = services.GetAllInstances<ITokenProvider>();
                    var provider = providers.FirstOrDefault(tokenProvider => tokenProvider.TokenFlow == tokenFlow);
                    provider.ClientConfiguration = clientConfiguration;
                    return new AuthorizationHandler(provider);
                })
                .AddHttpMessageHandler(c =>
                {
                    var correlationIdProvider = c.GetServices<ICorrelationIdProvider>().FirstOrDefault() 
                                                ?? new DefaultCorrelationIdProvider();
                    correlationIdProvider.ClientConfiguration = clientConfiguration;
                    return new CorrelationIdHandler(correlationIdProvider);
                })
                .AddHttpMessageHandler(c =>
                    new ErrorHandler(new ApiExceptionFactory(clientConfiguration, services.GetService<ISerializerService>())))
                .AddHttpMessageHandler(c => new LoggerHandler(services.GetService<ILoggerFactory>()));

            return httpClientBuilder;
        }

        private static void UseHttpApiDefaults(this Container services)
        {
            services.Register<ITokenStoreManager, InMemoryTokenStoreManager>(Lifestyle.Transient);
            services.Register<IUserCredentialsStoreManager, InMemoryUserCredentialsStoreManager>(Lifestyle.Transient);
            services.Register<IAnonymousCredentialsStoreManager, InMemoryAnonymousCredentialsStoreManager>(Lifestyle.Transient);
            services.RegisterCollection(typeof(IRequestMessageBuilder), typeof(IRequestMessageBuilder).Assembly);
            services.RegisterCollection(typeof(IAdditionalParametersBuilder), typeof(IAdditionalParametersBuilder).Assembly);
            services.RegisterCollection(typeof(ISearchParametersBuilder), typeof(ISearchParametersBuilder).Assembly);
            services.RegisterCollection(typeof(IQueryParametersBuilder), typeof(IQueryParametersBuilder).Assembly);
            services.RegisterCollection(typeof(IUploadImageParametersBuilder), typeof(IUploadImageParametersBuilder).Assembly);
            services.RegisterSingleton<ILoggerFactory>(new LoggerFactory());
            services.Register<IEndpointRetriever, EndpointRetriever>(Lifestyle.Singleton);
            services.Register<IParametersBuilderFactory<IAdditionalParametersBuilder>, ParametersBuilderFactory<IAdditionalParametersBuilder>>(Lifestyle.Singleton);
            services.Register<IParametersBuilderFactory<ISearchParametersBuilder>, ParametersBuilderFactory<ISearchParametersBuilder>>(Lifestyle.Singleton);
            services.Register<IParametersBuilderFactory<IQueryParametersBuilder>, ParametersBuilderFactory<IQueryParametersBuilder>>(Lifestyle.Singleton);
            services.Register<IParametersBuilderFactory<IUploadImageParametersBuilder>, ParametersBuilderFactory<IUploadImageParametersBuilder>>(Lifestyle.Singleton);
            services.Register<IHttpApiCommandFactory, HttpApiCommandFactory>(Lifestyle.Singleton);
            services.Register<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>(Lifestyle.Singleton);
            services.Register<IUserAgentProvider, UserAgentProvider>(Lifestyle.Singleton);
            services.Register<ITokenSerializerService, TokenSerializerService>(Lifestyle.Singleton);
            services.Register<ICorrelationIdProvider, DefaultCorrelationIdProvider>(Lifestyle.Singleton);
        }
        
        public static void ConfigureSerializationServices(
            this Container services,
            ISerializationConfiguration serializationConfiguration)
        {
            services.Options.AllowOverridingRegistrations = true;
            services.Register(()=> serializationConfiguration, Lifestyle.Singleton);
        }
    }
}
