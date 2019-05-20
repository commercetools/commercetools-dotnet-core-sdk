using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public static class SearchCommandExtensions
    {
        public static SearchCommand<T> Expand<T>(this SearchCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            if (command.SearchParameters != null && command.SearchParameters is IExpandable expandableParams)
            {
                expandableParams.Expand.Add(new Expansion<T>(expression).ToString());
            }

            return command;
        }

        public static SearchCommand<T> Expand<T>(this SearchCommand<T> command, string expression)
        {
            if (command.SearchParameters != null && command.SearchParameters is IExpandable expandableParams)
            {
                expandableParams.Expand.Add(expression);
            }

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

        public static SearchProductProjectionsCommand FilterQuery(this SearchProductProjectionsCommand command, string expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.FilterQuery.Add(expression);

            return command;
        }

        public static SearchProductProjectionsCommand Filter(this SearchProductProjectionsCommand command, string expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Filter.Add(expression);

            return command;
        }

        public static SearchProductProjectionsCommand FilterFacets(this SearchProductProjectionsCommand command, string expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.FilterFacets.Add(expression);

            return command;
        }

        public static SearchProductProjectionsCommand TermFacet(this SearchProductProjectionsCommand command, string expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(expression);

            return command;
        }

        public static SearchProductProjectionsCommand RangeFacet(this SearchProductProjectionsCommand command, string expression)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(expression);

            return command;
        }

        public static SearchProductProjectionsCommand Sort(this SearchProductProjectionsCommand command, string expression)
        {
            if (command.SearchParameters != null && command.SearchParameters is ISortable sortableParams)
            {
                sortableParams.Sort.Add(expression);
            }

            return command;
        }

        public static SearchProductProjectionsCommand Sort(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, IComparable>> expression, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (command.SearchParameters != null && command.SearchParameters is ISortable sortableParams)
            {
                sortableParams.Sort.Add(new Sort<ProductProjection>(expression, sortDirection).ToString());
            }

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
            if (command.SearchParameters != null && command.SearchParameters is IPageable pageableParams)
            {
                pageableParams.Limit = limit;
            }

            return command;
        }

        public static SearchProductProjectionsCommand Offset(this SearchProductProjectionsCommand command, int offset)
        {
            if (command.SearchParameters != null && command.SearchParameters is IPageable pageableParams)
            {
                pageableParams.Offset = offset;
            }

            return command;
        }

        public static SearchProductProjectionsCommand WithTotal(this SearchProductProjectionsCommand command, bool withTotal)
        {
            if (command.SearchParameters != null && command.SearchParameters is IPageable pageableParams)
            {
                pageableParams.WithTotal = withTotal;
            }

            return command;
        }
    }
}
