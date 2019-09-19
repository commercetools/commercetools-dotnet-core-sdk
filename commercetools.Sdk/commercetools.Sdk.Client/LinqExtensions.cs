using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;

namespace commercetools.Sdk.Client
{
    using System;

    [Obsolete("Experimental")]
    public static class LinqExtensions
    {
        public static ClientQueryableCollection<T> Query<T>(this IClient client)
        {
            return new ClientQueryableCollection<T>(client, new QueryCommand<T>());
        }

        public static ClientQueryableCollection<T> QueryAll<T>(this IClient client)
        {
            return new ClientQueryableCollection<T>(client, new QueryCommand<T>(), true);
        }

        public static ClientQueryableCollection<ProductProjection> SearchProducts(this IClient client)
        {
            return new ClientQueryableCollection<ProductProjection>(client, new SearchProductProjectionsCommand());
        }

        public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Reference>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    GetMethodInfo(Expand, source, expression),
                    new[] { source.Expression, expression }));
        }

        public static IQueryable<ProductProjection> Filter(this IQueryable<ProductProjection> source, Expression<Func<ProductProjection, bool>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<ProductProjection>(
                Expression.Call(
                    null,
                    GetMethodInfo(Filter, source, expression),
                    new[] { source.Expression, expression }));
        }

        public static IQueryable<ProductProjection> FilterQuery(this IQueryable<ProductProjection> source, Expression<Func<ProductProjection, bool>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<ProductProjection>(
                Expression.Call(
                    null,
                    GetMethodInfo(FilterQuery, source, expression),
                    new[] { source.Expression, expression }));
        }

        public static IQueryable<ProductProjection> FilterFacet(this IQueryable<ProductProjection> source, Expression<Func<ProductProjection, bool>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<ProductProjection>(
                Expression.Call(
                    null,
                    GetMethodInfo(FilterFacet, source, expression),
                    new[] { source.Expression, expression }));
        }

        public static IQueryable<ProductProjection> TermFacet(this IQueryable<ProductProjection> source, Expression<Func<ProductProjection, IComparable>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<ProductProjection>(
                Expression.Call(
                    null,
                    GetMethodInfo(TermFacet, source, expression),
                    new[] { source.Expression, expression }));
        }

        public static IQueryable<ProductProjection> RangeFacet(this IQueryable<ProductProjection> source, Expression<Func<ProductProjection, bool>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<ProductProjection>(
                Expression.Call(
                    null,
                    GetMethodInfo(RangeFacet, source, expression),
                    new[] { source.Expression, expression }));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801", Justification = "reflection optimization")]
        private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> f, T1 unused1, T2 unused2)
        {
            return f.Method;
        }
    }
}
