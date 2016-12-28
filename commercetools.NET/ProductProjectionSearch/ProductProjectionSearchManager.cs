using System.Text;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.ProductProjections;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// Provides access to the functions in the ProductProjectionSearch section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html"/>
    public class ProductProjectionSearchManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/product-projections/search";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public ProductProjectionSearchManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// This endpoint provides high performance search queries over ProductProjections.
        /// </summary>
        /// <remarks>
        /// The query result contains the ProductProjections for which at least one ProductVariant matches the search query. This means that variants can be included in the result also for which the search query does not match. To determine which ProductVariants match the search query, the returned ProductProjections include the additional field isMatchingVariant.
        /// </remarks>
        /// <param name="text">The text to analyze and search for, e.g. as supplied by a user through a search input field.</param>
        /// <param name="language">The language for the text parameter in form of an IETF language tag.</param>
        /// <param name="fuzzy">Whether to apply fuzzy search on the text to analyze</param>
        /// <param name="fuzzyLevel">Provide explicitly the fuzzy level desired if fuzzy is enabled. This value can not be higher than the one chosen by the platform by default.</param>
        /// <param name="filter">List of filters</param>
        /// <param name="filterQuery">List of filters</param>
        /// <param name="filterFacets">List of filters</param>
        /// <param name="facet">List of facets</param>
        /// <param name="sort">Sort</param>
        /// <param name="sortDirection">Sort direction</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <param name="staged">Whether to search in the current or staged projections</param>
        /// <param name="priceCurrency">Enables price selection</param>
        /// <param name="priceCountry">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <returns>ProductProjectionQueryResult object</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products-search.html#search-productprojections"/>
        public Task<Response<ProductProjectionQueryResult>> SearchProductProjectionsAsync(
            string text = null, 
            string language = null, 
            bool fuzzy = false,
            int fuzzyLevel = -1, 
            string[] filter = null,
            string[] filterQuery = null,
            string[] filterFacets = null,
            string[] facet = null,
            string sort = null,
            SortDirection sortDirection = SortDirection.Ascending, 
            int limit = -1, 
            int offset = -1, 
            bool staged = false, 
            string priceCurrency = null, 
            string priceCountry = null, 
            string priceCustomerGroup = null, 
            string priceChannel = null)
        {
            StringBuilder qs = new StringBuilder();

            qs.AppendFormat("?fuzzy={0}", fuzzy.ToString().ToLower());
            qs.AppendFormat("&staged={0}", staged.ToString().ToLower());

            if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(language))
            {
                string textKey = string.Concat("text.", language);
                qs.AppendFormat("&{0}={1}", textKey, Helper.UrlEncode(text));
            }

            if (fuzzyLevel > 0)
            {
                qs.AppendFormat("&fuzzyLevel={0}", fuzzyLevel);
            }

            if (filter != null)
            {
                for (int i = 0; i < filter.Length; i++)
                {
                    qs.AppendFormat("&filter={0}", Helper.UrlEncode(filter[i]));
                }
            }

            if (filterQuery != null)
            {
                for (int i = 0; i < filterQuery.Length; i++)
                {
                    qs.AppendFormat("&filter.query={0}", Helper.UrlEncode(filterQuery[i]));
                }
            }

            if (filterFacets != null)
            {
                for (int i = 0; i < filterFacets.Length; i++)
                {
                    qs.AppendFormat("&filter.facets={0}", Helper.UrlEncode(filterFacets[i]));
                }
            }

            if (facet != null)
            {
                for (int i = 0; i < facet.Length; i++)
                {
                    qs.AppendFormat("&facet={0}", Helper.UrlEncode(facet[i]));
                }
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    sort = string.Concat(sort, " asc");
                }
                else if (sortDirection == SortDirection.Descending)
                {
                    sort = string.Concat(sort, " desc");
                }

                qs.AppendFormat("&sort={0}", Helper.UrlEncode(sort));
            }

            if (limit > 0)
            {
                qs.AppendFormat("&limit={0}", limit);
            }

            if (offset > 0)
            {
                qs.AppendFormat("&offset={0}", offset);
            }

            if (!string.IsNullOrWhiteSpace(priceCurrency))
            {
                qs.AppendFormat("&priceCurrency={0}", Helper.UrlEncode(priceCurrency));

                if (!string.IsNullOrWhiteSpace(priceCountry))
                {
                    qs.AppendFormat("&priceCountry={0}", Helper.UrlEncode(priceCountry));
                }

                if (!string.IsNullOrWhiteSpace(priceCustomerGroup))
                {
                    qs.AppendFormat("&priceCustomerGroup={0}", Helper.UrlEncode(priceCustomerGroup));
                }

                if (!string.IsNullOrWhiteSpace(priceChannel))
                {
                    qs.AppendFormat("&priceChannel={0}", Helper.UrlEncode(priceChannel));
                }
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, qs.ToString());
            return _client.GetAsync<ProductProjectionQueryResult>(endpoint);
        }

        #endregion
    }
}
