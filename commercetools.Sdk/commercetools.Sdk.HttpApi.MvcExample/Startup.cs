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
            services.AddSingleton<IRequestMessageBuilder, QueryRequestMessageBuilder>();

            // TODO loop through classes and register this automatically
            IEnumerable<Type> registeredHttpApiCommandTypes = new List<Type>() { typeof(CreateHttpApiCommand<>), typeof(QueryHttpApiCommand<>), typeof(GetByIdHttpApiCommand<>), typeof(GetByKeyHttpApiCommand<>), typeof(UpdateByKeyHttpApiCommand<>), typeof(UpdateByIdHttpApiCommand<>), typeof(DeleteByKeyHttpApiCommand<>), typeof(DeleteByIdHttpApiCommand<>) };
            // TODO find a better way to add this to services, ienumerable is too generic
            services.AddSingleton<IEnumerable<Type>>(registeredHttpApiCommandTypes);
            services.AddSingleton<IHttpApiCommandFactory, HttpApiCommandFactory>();
            services.AddSingleton<IRequestMessageBuilderFactory, RequestMessageBuilderFactory>();

            //TODO Find a neater way to register all the converters
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, BooleanAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, DateAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, DateTimeAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, TimeAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, NumberAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, TextAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, EnumAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, LocalizedTextAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, LocalizedEnumAttributeConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Attribute>, MoneyAttributeConverter>();

            services.AddSingleton<ICustomConverter<Sdk.Domain.Money>, HighPrecisionMoneyConverter>();
            services.AddSingleton<ICustomConverter<Sdk.Domain.Money>, CentPrecisionMoneyConverter>();

            services.AddSingleton<MoneyConverter>();
            services.AddSingleton<AttributeConverter>();
            services.AddSingleton<CustomContractResolver>();
            services.AddSingleton<JsonSerializerSettingsFactory>();
            services.AddSingleton<ISerializerService, SerializerService>();
            services.AddSingleton<IClient, Client>();

            services.AddMvc();
        }
    }
}