using commercetools.Sdk.Serialization;
using commercetools.Sdk.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.UseSerialization();
            services.UseHttpApiWithClientCredentials(this.configuration, "Client");

            services.AddMvc();
            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
        }
    }
}