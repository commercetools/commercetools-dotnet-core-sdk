using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.MvcExample
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ClientConfiguration clientConfiguration = this.configuration.GetSection("Client").Get<ClientConfiguration>();
            services.AddSingleton<IClientConfiguration>(clientConfiguration);

            services.AddSingleton<ITokenStoreManager, InMemoryTokenStoreManager>();
            services.AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            services.AddSingleton<ITokenFlowRegister>(tokenFlowRegister);
            services.AddSingleton<AuthorizationHandler>();

            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>();
            // TODO Find automated way to register all message builders
            IDictionary<Type, Type> registeredRequestMessageBuilders = new Dictionary<Type, Type>();
            registeredRequestMessageBuilders.Add(typeof(GetByIdCommand), typeof(GetByIdRequestMessageBuilder));
            registeredRequestMessageBuilders.Add(typeof(GetByKeyCommand), typeof(GetByKeyRequestMessageBuilder));
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(registeredRequestMessageBuilders);
            services.AddSingleton<IRequestMessageBuilderFactory>(requestMessageBuilderFactory);
            services.AddSingleton<IRequestBuilder, RequestBuilder>();
            services.AddSingleton<Func<Type, JsonSerializerSettings>>(JsonSerializerSettingsFactory.Create);
            services.AddSingleton<ISerializerService, SerializerService>();
            services.AddSingleton<IClient, Client>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}