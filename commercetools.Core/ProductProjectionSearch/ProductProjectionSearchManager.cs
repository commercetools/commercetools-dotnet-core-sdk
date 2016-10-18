using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        /// <param name="text">Text to search for</param>
        /// <param name="language">Language of the text</param>
        /// <param name="fuzzy">Whether to apply fuzzy search on the text to analyze</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <param name="staged">Whether to search in the current or staged projections</param>
        /// <param name="priceCurrency">Enables price selection</param>
        /// <param name="priceCountry">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <returns>ProductProjectionQueryResult object</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products-search.html#search-productprojections"/>
        public async Task<ProductProjectionQueryResult> SearchProductProjectionsAsync(string text = null, string language = null, bool fuzzy = false, string sort = null, int limit = -1, int offset = -1, bool staged = false, string priceCurrency = null, string priceCountry = null, string priceCustomerGroup = null, string priceChannel = null)
        {
            NameValueCollection values = new NameValueCollection
            {
                { "fuzzy", fuzzy.ToString() },
                { "staged", staged.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(language))
            {
                string textKey = string.Concat("text.", language);
                values.Add(textKey, text);
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                values.Add("sort", sort);
            }

            if (limit > 0)
            {
                values.Add("limit", limit.ToString());
            }

            if (offset >= 0)
            {
                values.Add("offset", offset.ToString());
            }

            if (!string.IsNullOrWhiteSpace(priceCurrency))
            {
                values.Add("priceCurrency", priceCurrency);

                if (!string.IsNullOrWhiteSpace(priceCountry))
                {
                    values.Add("priceCountry", priceCountry);
                }

                if (!string.IsNullOrWhiteSpace(priceCustomerGroup))
                {
                    values.Add("priceCustomerGroup", priceCustomerGroup);
                }

                if (!string.IsNullOrWhiteSpace(priceChannel))
                {
                    values.Add("priceChannel", priceChannel);
                }
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX);
            dynamic response = await _client.GetAsync(endpoint, values);

            return new ProductProjectionQueryResult(response);
        }

        #endregion
    }
}