using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.AnonymousSessionExample
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
            services.UseCommercetools(this.configuration, "Client", TokenFlow.AnonymousSession);
            services.AddSingleton<IAnonymousCredentialsStoreManager, AnonymousCredentialsStoreManager>();
            services.AddMvc();
        }
    }
}