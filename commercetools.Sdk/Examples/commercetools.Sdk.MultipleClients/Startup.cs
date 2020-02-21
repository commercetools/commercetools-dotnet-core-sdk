using System.Collections.Concurrent;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace commercetools.Sdk.MultipleClients
{
    public class Startup
    {
        private readonly IConfiguration configuration;

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
            IDictionary<string, TokenFlow> clients = new ConcurrentDictionary<string, TokenFlow>();
            clients.TryAdd("client1", TokenFlow.AnonymousSession);
            clients.TryAdd("client2", TokenFlow.ClientCredentials);
            
            services.UseCommercetools(this.configuration, clients);
            services.AddHttpContextAccessor();
            services.AddSingleton<IUserCredentialsStoreManager, UserCredentialsStoreManager>();
            services.AddSingleton<IAnonymousCredentialsStoreManager, AnonymousCredentialsStoreManager>();
            services.AddMvc();
        }
    }
}
