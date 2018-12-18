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
            services.RegisterAllTypes<IQueryPredicateVisitorConverter>(ServiceLifetime.Singleton);
            services.AddSingleton<QueryPredicateVisitorFactory>();
            services.AddSingleton<IQueryPredicateExpressionVisitor, QueryPredicateExpressionVisitor>();

            services.RegisterAllTypes<IFilterPredicateVisitorConverter>(ServiceLifetime.Singleton);
            services.AddSingleton<FilterPredicateVisitorFactory>();
            services.AddSingleton<IFilterPredicateExpressionVisitor, FilterPredicateExpressionVisitor>();

            services.RegisterAllTypes<IDiscountPredicateVisitorConverter>(ServiceLifetime.Singleton);
            services.AddSingleton<DiscountPredicateVisitorFactory>();
            services.AddSingleton<IDiscountPredicateExpressionVisitor, DiscountPredicateExpressionVisitor>();

            services.AddSingleton<IExpansionExpressionVisitor, ExpansionExpressionVisitor>();
            services.AddSingleton<ISortExpressionVisitor, SortExpressionVisitor>();
        }
    }
}
