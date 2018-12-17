using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Registration
{
    public static class DependencyInjectionSetup
    {
        public static void UseRegistration(this IServiceCollection services)
        {
            services.AddSingleton<IRegisteredTypeRetriever, RegisteredTypeRetriever>();
        }
    }
}