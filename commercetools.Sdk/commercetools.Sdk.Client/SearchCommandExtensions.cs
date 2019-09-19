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

        public static void SetStaged<T>(this SearchCommand<T> command, bool staged)
        {
            if (command.AdditionalParameters == null)
            {
                command.AdditionalParameters = new ProductProjectionAdditionalParameters();
            }

            if (command.AdditionalParameters is ProductProjectionAdditionalParameters parameters)
            {
                parameters.Staged = staged;
                return;
            }

            throw new ArgumentException("AdditionalParameters not of type ProductProjectionAdditionalParameters");
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

        public static SearchProductProjectionsCommand TermFacet(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, IComparable>> expression, bool isCountingProducts = false)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(new TermFacet<ProductProjection>(expression) { IsCountingProducts = isCountingProducts }.ToString());

            return command;
        }

        public static SearchProductProjectionsCommand RangeFacet(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, bool>> expression, bool isCountingProducts = false)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(new RangeFacet<ProductProjection>(expression) { IsCountingProducts = isCountingProducts }.ToString());

            return command;
        }

        public static SearchProductProjectionsCommand TermFacet(this SearchProductProjectionsCommand command, string alias, Expression<Func<ProductProjection, IComparable>> expression, bool isCountingProducts = false)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            var termFacet = new TermFacet<ProductProjection>(expression) { Alias = alias, IsCountingProducts = isCountingProducts };
            p.Facets.Add(termFacet.ToString());

            return command;
        }

        public static SearchProductProjectionsCommand FilteredFacet(this SearchProductProjectionsCommand command, Expression<Func<ProductProjection, bool>> expression, bool isCountingProducts = false)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.Facets.Add(new FilterFacet<ProductProjection>(expression) { IsCountingProducts = isCountingProducts }.ToString());

            return command;
        }

        public static SearchProductProjectionsCommand FilteredFacet(this SearchProductProjectionsCommand command, string alias, Expression<Func<ProductProjection, bool>> expression, bool isCountingProducts = false)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            var filteredFacet = new FilterFacet<ProductProjection>(expression) { Alias = alias, IsCountingProducts = isCountingProducts };
            p.Facets.Add(filteredFacet.ToString());
            return command;
        }

        public static SearchProductProjectionsCommand RangeFacet(this SearchProductProjectionsCommand command, string alias, Expression<Func<ProductProjection, bool>> expression, bool isCountingProducts = false)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            var rangeFacet = new RangeFacet<ProductProjection>(expression) { Alias = alias, IsCountingProducts = isCountingProducts };
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
                // Set limit to lowest value
                // Case: query.Take(5).Take(10) should still yield only 5 items
                if (pageableParams.Limit == null)
                {
                    pageableParams.Limit = limit;
                }
                else
                {
                    pageableParams.Limit = Math.Min(pageableParams.Limit.Value, limit);
                }
            }

            return command;
        }

        public static SearchProductProjectionsCommand Offset(this SearchProductProjectionsCommand command, int offset)
        {
            if (command.SearchParameters != null && command.SearchParameters is IPageable pageableParams)
            {
                // Sum all skips together
                // Case: query.Skip(5).Skip(10) should result in an offset of 15
                if (pageableParams.Offset == null)
                {
                    pageableParams.Offset = offset;
                }
                else
                {
                    pageableParams.Offset += offset;
                }

                // Case: query.Take(3).Skip(2) should yield only 1 item
                // Case: query.Take(2).Skip(1).Take(2) should yield 0 items
                // Case: query.Skip(3).Take(1).Skip(1) should yield 0 items
                // This case is only of relevance if at least one Take operation was done beforehand
                if (pageableParams.Limit != null)
                {
                    pageableParams.Limit -= offset;
                    pageableParams.Limit = Math.Max(0, pageableParams.Limit.Value);
                }
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

        public static SearchProductProjectionsCommand SetPriceCurrency(this SearchProductProjectionsCommand command, string priceCurrency)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.PriceCurrency = priceCurrency;

            return command;
        }

        public static SearchProductProjectionsCommand SetPriceCountry(this SearchProductProjectionsCommand command, string priceCountry)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.PriceCountry = priceCountry;

            return command;
        }

        public static SearchProductProjectionsCommand SetPriceCustomerGroup(this SearchProductProjectionsCommand command, string priceCustomerGroup)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.PriceCustomerGroup = priceCustomerGroup;

            return command;
        }

        public static SearchProductProjectionsCommand SetPriceChannel(this SearchProductProjectionsCommand command, string priceChannel)
        {
            var p = command.SearchParameters as ProductProjectionSearchParameters;
            p.PriceChannel = priceChannel;

            return command;
        }
    }
}
