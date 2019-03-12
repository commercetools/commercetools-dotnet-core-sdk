using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    using System;

    [Obsolete("Experimental")]
    public static class Extensions
    {
        public static ClientQueryable<T> Query<T>(this IClient client)
        {
            return new ClientQueryable<T>(client, new QueryCommand<T>());
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

        public static QueryCommand<T> Where<T>(this QueryCommand<T> command, Expression<Func<T, bool>> expression)
        {
            command.SetWhere(new QueryPredicate<T>(expression));

            return command;
        }

        public static GetCommand<T> Expand<T>(this GetCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            command.Expand.Add(new Expansion<T>(expression).ToString());

            return command;
        }

        public static QueryCommand<T> Expand<T>(this QueryCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            command.Expand.Add(new Expansion<T>(expression).ToString());

            return command;
        }

        public static QueryCommand<T> Sort<T>(this QueryCommand<T> command, Expression<Func<T, IComparable>> expression, SortDirection sortDirection = SortDirection.Ascending)
        {
            command.Sort.Add(new Sort<T>(expression, sortDirection).ToString());

            return command;
        }


        public static SearchProductProjectionsCommand FilterQuery(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, bool>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.FilterQuery.Add(new Filter<ProductProjection>(expression).ToString());

            return command;
        }

        public static SearchProductProjectionsCommand Filter(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, bool>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Filter.Add(new Filter<ProductProjection>(expression).ToString());

            return command;
        }

        public static SearchProductProjectionsCommand FilterFacets(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, bool>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.FilterFacets.Add(new Filter<ProductProjection>(expression).ToString());

            return command;
        }

        public static SearchProductProjectionsCommand TermFacet(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, IComparable>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(new TermFacet<ProductProjection>(expression).ToString());

            return command;
        }

        public static SearchProductProjectionsCommand RangeFacet(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, bool>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(new RangeFacet<ProductProjection>(expression).ToString());

            return command;
        }

        public static SearchProductProjectionsCommand TermFacet(this SearchProductProjectionsCommand command, string alias, Expression<Func<ProductProjection, IComparable>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            var termFacet = new TermFacet<ProductProjection>(expression);
            termFacet.Alias = alias;
            p.Facets.Add(termFacet.ToString());

            return command;
        }

        public static SearchProductProjectionsCommand RangeFacet(this SearchProductProjectionsCommand command, string alias, Expression<Func<ProductProjection, bool>> expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            var rangeFacet = new RangeFacet<ProductProjection>(expression);
            rangeFacet.Alias = alias;
            p.Facets.Add(rangeFacet.ToString());

            return command;
        }

        public static SearchProductProjectionsCommand Sort(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, IComparable>> expression, SortDirection sortDirection = SortDirection.Ascending)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Sort.Add(new Sort<ProductProjection>(expression, sortDirection).ToString());

            return command;
        }

        public static SearchProductProjectionsCommand Fuzzy(this SearchProductProjectionsCommand command, bool fuzzy)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Fuzzy = fuzzy;

            return command;
        }

        public static SearchProductProjectionsCommand MarkMatchingVariants(this SearchProductProjectionsCommand command, bool markMatchingVariants)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.MarkMatchingVariants = markMatchingVariants;

            return command;
        }

        public static SearchProductProjectionsCommand FuzzyLevel(this SearchProductProjectionsCommand command, int fuzzyLevel)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Fuzzy = fuzzyLevel >= 0;
            p.FuzzyLevel = Math.Min(Math.Max(fuzzyLevel, 0), 2);

            return command;
        }

        public static SearchProductProjectionsCommand Limit(this SearchProductProjectionsCommand command, int limit)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Limit = limit;

            return command;
        }

        public static SearchProductProjectionsCommand Offset(this SearchProductProjectionsCommand command, int offset)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Offset = offset;

            return command;
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> f, T1 unused1, T2 unused2)
        {
            return f.Method;
        }
    }
}
