using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Linq.Filter;
using commercetools.Sdk.Linq.Query;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Linq
{
    public static class DependencyInjectionSetup
    {
        public static void UseLinq(this IServiceCollection services)
        {
            services.RegisterAllInterfaceTypes<IQueryPredicateVisitorConverter>(ServiceLifetime.Singleton);
            services.AddSingleton<QueryPredicateVisitorFactory>();
            services.AddSingleton<IQueryPredicateExpressionVisitor, QueryPredicateExpressionVisitor>();

            services.RegisterAllInterfaceTypes<IFilterPredicateVisitorConverter>(ServiceLifetime.Singleton);
            services.AddSingleton<FilterPredicateVisitorFactory>();
            services.AddSingleton<IFilterPredicateExpressionVisitor, FilterPredicateExpressionVisitor>();

            services.RegisterAllInterfaceTypes<IDiscountPredicateVisitorConverter>(ServiceLifetime.Singleton);
            services.AddSingleton<DiscountPredicateVisitorFactory>();
            services.AddSingleton<IDiscountPredicateExpressionVisitor, DiscountPredicateExpressionVisitor>();
        }
    }
}
