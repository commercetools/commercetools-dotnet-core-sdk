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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
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
            // TODO Auto register all message builders by looping through interface implementations
            services.AddSingleton<IRequestMessageBuilder, CreateRequestMessageBuilder>();
            services.AddSingleton<IRequestMessageBuilder, UpdateByIdRequestMessageBuilder>();
            services.AddSingleton<IRequestMessageBuilder, UpdateByKeyRequestMessageBuilder>();
            services.AddSingleton<IRequestMessageBuilder, GetByIdRequestMessageBuilder>();
            services.AddSingleton<IRequestMessageBuilder, GetByKeyRequestMessageBuilder>();
            services.AddSingleton<IRequestMessageBuilder, DeleteByIdRequestMessageBuilder>();
            services.AddSingleton<IRequestMessageBuilder, DeleteByKeyRequestMessageBuilder>();

            IDictionary<Type, Type> mapping = new Dictionary<Type, Type>();
            mapping.Add(typeof(GetByIdCommand<>), typeof(GetByIdRequestMessageBuilder));
            mapping.Add(typeof(GetByKeyCommand<>), typeof(GetByKeyRequestMessageBuilder));
            mapping.Add(typeof(UpdateByIdCommand<>), typeof(UpdateByIdRequestMessageBuilder));
            mapping.Add(typeof(UpdateByKeyCommand<>), typeof(UpdateByKeyRequestMessageBuilder));
            mapping.Add(typeof(DeleteByIdCommand<>), typeof(DeleteByIdRequestMessageBuilder));
            mapping.Add(typeof(DeleteByKeyCommand<>), typeof(DeleteByKeyRequestMessageBuilder));
            mapping.Add(typeof(CreateCommand<>), typeof(CreateRequestMessageBuilder));
            // TODO Find a better way of registering this dictionary
            services.AddSingleton<IDictionary<Type, Type>>(mapping);

            services.AddSingleton<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>();
            services.AddSingleton<IRequestBuilder, RequestBuilder>();
            services.AddSingleton<Func<Type, JsonSerializerSettings>>(JsonSerializerSettingsFactory.Create);
            services.AddSingleton<ISerializerService, SerializerService>();
            services.AddSingleton<IClient, Client>();

            services.AddMvc();
        }
    }
}