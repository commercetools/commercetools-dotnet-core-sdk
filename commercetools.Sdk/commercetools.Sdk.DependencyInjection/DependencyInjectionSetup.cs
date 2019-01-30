using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace commercetools.Sdk.DependencyInjection
{
    public static class DependencyInjectionSetup
    {
        public static void UseCommercetools(this IServiceCollection services, IConfiguration configuration, string clientName = DefaultClientNames.Api, TokenFlow tokenFlow = TokenFlow.ClientCredentials)
        {
            services.UseCommercetools(configuration, new Dictionary<string, TokenFlow>() { { clientName, tokenFlow } });
        }

        public static void UseCommercetools(this IServiceCollection services, IConfiguration configuration, IDictionary<string, TokenFlow> clients)
        {
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            services.UseLinq();
            services.UseHttpApi(configuration, clients);
            // TODO In case service locator pattern is completely remove, remove these two lines as well
            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetServiceLocatorProvider(serviceProvider);
        }
    }
}
