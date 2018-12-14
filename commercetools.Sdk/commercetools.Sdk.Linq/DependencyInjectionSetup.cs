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
        }
    }
}
