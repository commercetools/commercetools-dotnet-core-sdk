using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Validation
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionSetup
    {
        public static void UseValidation(this IServiceCollection services)
        {
            services.RegisterAllTypes(typeof(IResourceValidator), ServiceLifetime.Singleton);
            services.AddSingleton<IModelValidator, ModelValidator>();
        }
    }
}
