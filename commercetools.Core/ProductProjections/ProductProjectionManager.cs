using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

namespace commercetools.ProductProjections
{
    /// <summary>
    /// Provides access to the functions in the ProductProjections section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html"/>
    public class ProductProjectionManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/product-projections";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public ProductProjectionManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets the current or staged representation of a product in a catalog by ID.
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>ProductProjection</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#get-productprojection-by-id"/>
        public async Task<ProductProjection> GetProductProjectionByIdAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("productId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productId);
            dynamic response = await _client.GetAsync(endpoint);

            return new ProductProjection(response);
        }

        /// <summary>
        /// Gets the current or staged representation of a product found by Key.
        /// </summary>
        /// <param name="key">Product key</param>
        /// <returns>ProductProjection</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#get-productprojection-by-key"/>
        public async Task<ProductProjection> GetProductProjectionByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            dynamic response = await _client.GetAsync(endpoint);

            return new ProductProjection(response);
        }

        /// <summary>
        /// You can use the product projections query endpoint to get the current or staged representations of Products.
        /// </summary>
        /// <remarks>
        /// We suggest to use the performance optimized search endpoint which has a bunch functionalities, the query API lacks, like sorting on custom attributes, etc.
        /// </remarks>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <param name="staged">Whether to query for current or staged projections</param>
        /// <returns>ProductProjectionQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#query-productprojections"/>
        public async Task<ProductProjectionQueryResult> QueryProductProjectionsAsync(string where = null, string sort = null, int limit = -1, int offset = -1, bool staged = false)
        {
            NameValueCollection values = new NameValueCollection
            {
                { "staged", staged.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(where))
            {
                values.Add("where", where);
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

            dynamic response = await _client.GetAsync(ENDPOINT_PREFIX, values);

            return new ProductProjectionQueryResult(response);
        }

        #endregion
    }
}