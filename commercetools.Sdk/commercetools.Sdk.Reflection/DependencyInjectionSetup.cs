namespace commercetools.Sdk.Util
{
    using commercetools.Sdk.Util;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionSetup
    {
        public static void UseUtil(this IServiceCollection services)
        {
            services.AddSingleton<IRegisteredTypeRetriever, RegisteredTypeRetriever>();
        }
    }
}