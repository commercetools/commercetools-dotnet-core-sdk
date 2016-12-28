using System;
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
        /// <param name="staged">Whether to query for the current or staged projections.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>ProductProjection</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#get-productprojection-by-id"/>
        public Task<Response<ProductProjection>> GetProductProjectionByIdAsync(string productId, bool staged = false, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("productId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productId);
            return GetProductProjectionAsync(endpoint, staged, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// Gets the current or staged representation of a product found by Key.
        /// </summary>
        /// <param name="key">Product key</param>
        /// <param name="staged">Whether to query for the current or staged projections.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>ProductProjection</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#get-productprojection-by-key"/>
        public Task<Response<ProductProjection>> GetProductProjectionByKeyAsync(string key, bool staged = false, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return GetProductProjectionAsync(endpoint, staged, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// Private worker method for GetProductProjectionByIdAsync and GetProductProjectionByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="staged">Whether to query for the current or staged projections.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#get-productprojection"/>
        private Task<Response<ProductProjection>> GetProductProjectionAsync(string endpoint, bool staged = false, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            NameValueCollection values = new NameValueCollection
            {
                { "staged", staged.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(priceCurrency))
            {
                values.Add("priceCurrency", priceCurrency);

                if (!string.IsNullOrWhiteSpace(priceCountry))
                {
                    values.Add("priceCountry", priceCountry);
                }

                if (priceCustomerGroup != Guid.Empty)
                {
                    values.Add("priceCustomerGroup", priceCustomerGroup.ToString());
                }

                if (priceChannel != Guid.Empty)
                {
                    values.Add("priceChannel", priceChannel.ToString());
                }
            }

            return _client.GetAsync<ProductProjection>(endpoint, values);
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
        /// <param name="staged">Whether to query for the current or staged projections.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>ProductProjectionQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#query-productprojections"/>
        public Task<Response<ProductProjectionQueryResult>> QueryProductProjectionsAsync(
            string where = null, 
            string sort = null, 
            int limit = -1, 
            int offset = -1, 
            bool staged = false,
            string priceCurrency = null,
            string priceCountry = null,
            string priceCustomerGroup = null,
            string priceChannel = null)
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

            return _client.GetAsync<ProductProjectionQueryResult>(ENDPOINT_PREFIX, values);
        }

        #endregion
    }
}
