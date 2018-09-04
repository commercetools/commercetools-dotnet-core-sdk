using commercetools.Sdk.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            services.AddHttpContextAccessor();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>();
            
            services.AddSingleton<AuthorizationHandler>();           

            services.AddHttpClient("auth");
            services.AddHttpClient("api").AddHttpMessageHandler<AuthorizationHandler>();

            services.AddSingleton<IClient, Client>();            

            services.AddMvc();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();

            app.UseMvcWithDefaultRoute();
        }
    }
}