using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.DependencyInjection
{
    public static class DependencyInjectionSetup
    {
        public static void UseCommercetoolsWithClientCredentials(this IServiceCollection services, IConfiguration configuration, string configurationSection)
        {
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            services.UseLinq();
            services.UseHttpApiWithClientCredentials(configuration, configurationSection);
            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetServiceLocatorProvider(serviceProvider);
        }

        public static void UseCommercetoolsWithPassword(this IServiceCollection services, IConfiguration configuration, string configurationSection)
        {
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            services.UseLinq();
            services.UseHttpApiWithPassword(configuration, configurationSection);
            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetServiceLocatorProvider(serviceProvider);
        }
    }
}
