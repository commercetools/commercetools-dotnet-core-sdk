using commercetools.Sdk.Domain.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Domain
{
    public static class DependencyInjectionSetup
    {
        public static void UseDomain(this IServiceCollection services)
        {
            services.AddSingleton<ICountryValidator, CountryValidator>();
            services.AddSingleton<ICurrencyValidator, CurrencyValidator>();
            services.AddSingleton<ICultureValidator, CultureValidator>();
            services.AddSingleton<IModelValidator, ModelValidator>();
        }
    }
}