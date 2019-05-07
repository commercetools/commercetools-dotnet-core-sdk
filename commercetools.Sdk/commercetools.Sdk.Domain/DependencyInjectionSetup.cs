using System.Linq;
using commercetools.Sdk.Domain.Validation;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Linq.Filter;
using commercetools.Sdk.Linq.Query;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Domain
{
    public static class DependencyInjectionSetup
    {
        public static void UseDomain(this IServiceCollection services)
        {
            services.AddSingleton<ICultureValidator, CultureValidator>();

            services.SetupExtension();
        }

        public static void SetupExtension(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ExpressionExtensions.SortExpressionVisitor = serviceProvider.GetService<ISortExpressionVisitor>();
            ExpressionExtensions.ExpansionExpressionVisitor = serviceProvider.GetService<IExpansionExpressionVisitor>();
            ExpressionExtensions.QueryPredicateExpressionVisitor =
                serviceProvider.GetService<IQueryPredicateExpressionVisitor>();
            ExpressionExtensions.FilterPredicateExpressionVisitor =
                serviceProvider.GetService<IFilterPredicateExpressionVisitor>();
            ExpressionExtensions.DiscountPredicateExpressionVisitor =
                serviceProvider.GetService<IDiscountPredicateExpressionVisitor>();
        }
    }
}
