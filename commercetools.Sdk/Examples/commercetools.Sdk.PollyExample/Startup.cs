using System.Collections.Generic;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace commercetools.Sdk.PollyExample
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddControllersWithViews();

            //Creating the policy
            var registry = services.AddPolicyRegistry();
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                .RetryAsync(3, onRetry: (exception, retryCount, context) =>
                {
                    var exceptionText = exception?.Exception == null
                        ? string.Empty
                        : $" exception:{exception.Exception.Message}";
                    // logging here
                });
            registry.Add("commercetoolsRetryPolicy", policy);

            var clients = new Dictionary<string, TokenFlow>()
            {
                {"AdminClient", TokenFlow.ClientCredentials},
                {"AnonymousClient", TokenFlow.AnonymousSession}
            };
            // services.UseCommercetools(configuration, "AdminClient")
            //     .AddPolicyHandlerFromRegistry("commercetoolsRetryPolicy");
            services.UseCommercetools(configuration, clients)
                .ConfigureAllClients(
                    builder => builder.AddPolicyHandlerFromRegistry("commercetoolsRetryPolicy"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Categories}/{action=Index}/{id?}");
            });
        }
    }
}