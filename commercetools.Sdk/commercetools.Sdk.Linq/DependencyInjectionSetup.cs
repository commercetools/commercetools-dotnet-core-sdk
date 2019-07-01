using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Linq.Filter;
using commercetools.Sdk.Linq.Query;
using commercetools.Sdk.Linq.Sort;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Linq
{
    public static class DependencyInjectionSetup
    {
        public static void UseLinq(this IServiceCollection services)
        {
            services.AddSingleton<IQueryPredicateExpressionVisitor, QueryPredicateExpressionVisitor>();
            services.AddSingleton<IFilterPredicateExpressionVisitor, FilterPredicateExpressionVisitor>();
            services.AddSingleton<IDiscountPredicateExpressionVisitor, DiscountPredicateExpressionVisitor>();

            services.AddSingleton<IExpansionExpressionVisitor, ExpansionExpressionVisitor>();
            services.AddSingleton<ISortPredicateExpressionVisitor, SortPredicateExpressionVisitor>();
        }
    }
}
