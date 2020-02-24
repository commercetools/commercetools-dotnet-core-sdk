﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
        public static IDictionary<string, IHttpClientBuilder> UseHttpApi(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.UseHttpApiDefaults();

            if (clients.Count() == 1)
            {
                return services.UseSingleClient(configuration, clients.First().Key, clients.First().Value);
            }

            return services.UseMultipleClients(configuration, clients);
        }

        private static IDictionary<string, IHttpClientBuilder> UseMultipleClients(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.AddSingleton<ICtpClientFactory, CtpClientFactory>();
            var builders = new ConcurrentDictionary<string, IHttpClientBuilder>();
            foreach (KeyValuePair<string, TokenFlow> client in clients)
            {
                string clientName = client.Key;
                TokenFlow tokenFlow = client.Value;

                IClientConfiguration clientConfiguration = configuration.GetSection(clientName).Get<ClientConfiguration>();
                Validator.ValidateObject(clientConfiguration, new ValidationContext(clientConfiguration), true);

                builders.TryAdd(clientName, services.SetupClient(clientName, clientConfiguration, tokenFlow));
                services.AddSingleton(c => c.GetService<ICtpClientFactory>().Create(clientName));
            }

            return builders;
        }

        private static IDictionary<string, IHttpClientBuilder> UseSingleClient(this IServiceCollection services, IConfiguration configuration, string clientName, TokenFlow tokenFlow)
        {
            services.AddSingleton<ICtpClientFactory, CtpClientFactory>();
            IClientConfiguration clientConfiguration = configuration.GetSection(clientName).Get<ClientConfiguration>();
            Validator.ValidateObject(clientConfiguration, new ValidationContext(clientConfiguration), true);

            services.AddSingleton(c => c.GetService<ICtpClientFactory>().Create(clientName));

            var builders = new ConcurrentDictionary<string, IHttpClientBuilder>();
            builders.TryAdd(clientName, services.SetupClient(clientName, clientConfiguration, tokenFlow));

            return builders;
        }

        private static IHttpClientBuilder SetupClient(this IServiceCollection services, string clientName, IClientConfiguration clientConfiguration, TokenFlow tokenFlow)
        {
            var httpClientBuilder = services.AddHttpClient(clientName)
                .ConfigureHttpClient((c, client) =>
                {
                    client.BaseAddress =
                        new Uri(clientConfiguration.ApiBaseAddress + clientConfiguration.ProjectKey + "/");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(c.GetService<IUserAgentProvider>().UserAgent);
                })
                .AddHttpMessageHandler(c =>
                {
                    var providers = c.GetServices<ITokenProvider>();
                    var provider = providers.FirstOrDefault(tokenProvider => tokenProvider.TokenFlow == tokenFlow);
                    provider.ClientConfiguration = clientConfiguration;
                    return new AuthorizationHandler(provider);
                })
                .AddHttpMessageHandler(c =>
                    new CorrelationIdHandler(new DefaultCorrelationIdProvider(clientConfiguration)))
                .AddHttpMessageHandler(c =>
                    new ErrorHandler(new ApiExceptionFactory(clientConfiguration, c.GetService<ISerializerService>())))
                .AddHttpMessageHandler<LoggerHandler>();

            return httpClientBuilder;
        }

        private static void UseHttpApiDefaults(this IServiceCollection services)
        {
            services.AddHttpClient(DefaultClientNames.Authorization);
            services.AddTransient<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddTransient<IUserCredentialsStoreManager, InMemoryUserCredentialsStoreManager>();
            services.AddTransient<IAnonymousCredentialsStoreManager, InMemoryAnonymousCredentialsStoreManager>();
            services.AddTransient<LoggerHandler>();
            services.RegisterAllTypes<ITokenProvider>(ServiceLifetime.Transient);
            services.RegisterAllTypes<IRequestMessageBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<IAdditionalParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<IQueryParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<ISearchParametersBuilder>(ServiceLifetime.Singleton);
            services.RegisterAllTypes<IUploadImageParametersBuilder>(ServiceLifetime.Singleton);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<IEndpointRetriever, EndpointRetriever>();
            services.AddSingleton<IParametersBuilderFactory<IAdditionalParametersBuilder>, ParametersBuilderFactory<IAdditionalParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<ISearchParametersBuilder>, ParametersBuilderFactory<ISearchParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<IQueryParametersBuilder>, ParametersBuilderFactory<IQueryParametersBuilder>>();
            services.AddSingleton<IParametersBuilderFactory<IUploadImageParametersBuilder>, ParametersBuilderFactory<IUploadImageParametersBuilder>>();
            services.AddSingleton<IHttpApiCommandFactory, HttpApiCommandFactory>();
            services.AddSingleton<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>();
            services.AddSingleton<IUserAgentProvider, UserAgentProvider>();
            services.AddSingleton<ITokenSerializerService, TokenSerializerService>();
        }
    }
}
