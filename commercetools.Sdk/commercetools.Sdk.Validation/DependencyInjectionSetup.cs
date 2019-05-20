using commercetools.Sdk.Domain.Validation;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace commercetools.Sdk.Validation
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionSetup
    {
        public static void UseValidation(this IServiceCollection services)
        {
            services.RegisterAllTypes(typeof(IResourceValidator), ServiceLifetime.Singleton);
            services.Replace(
                new ServiceDescriptor(
                    typeof(IModelValidator),
                    typeof(ModelValidator),
                    ServiceLifetime.Singleton)
                );
        }
    }
}
