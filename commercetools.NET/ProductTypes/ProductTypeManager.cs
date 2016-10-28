using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// Provides access to the functions in the ProductTypes section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html"/>
    public class ProductTypeManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/product-types";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public ProductTypeManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a ProductType by its ID.
        /// </summary>
        /// <param name="productTypeId">Product type ID</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#get-a-producttype-by-id"/>
        /// <returns>ProductType</returns>
        public async Task<ProductType> GetProductTypeByIdAsync(string productTypeId)
        {
            if (string.IsNullOrWhiteSpace(productTypeId))
            {
                throw new ArgumentException("productTypeId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productTypeId);
            dynamic response = await _client.GetAsync(endpoint);

            return new ProductType(response);
        }

        /// <summary>
        /// Gets a ProductType by its key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#get-a-producttype-by-key"/>
        /// <returns>ProductType</returns>
        public async Task<ProductType> GetProductTypeByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", Helper.UrlEncode(key));
            dynamic response = await _client.GetAsync(endpoint);

            return new ProductType(response);
        }

        /// <summary>
        /// Queries for ProductTypes.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>ProductTypeQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#query-producttypes"/>
        public async Task<ProductTypeQueryResult> QueryProductTypesAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
        {
            NameValueCollection values = new NameValueCollection();

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

            return new ProductTypeQueryResult(response);
        }

        /// <summary>
        /// Creates a new ProductType.
        /// </summary>
        /// <param name="productTypeDraft">ProductTypeDraft</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#create-a-producttype"/>
        public async Task<ProductType> CreateProductTypeAsync(ProductTypeDraft productTypeDraft)
        {
            if (string.IsNullOrWhiteSpace(productTypeDraft.Name))
            {
                throw new ArgumentException("name is required");
            }

            if (string.IsNullOrWhiteSpace(productTypeDraft.Description))
            {
                throw new ArgumentException("description is required");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string payload = JsonConvert.SerializeObject(productTypeDraft, settings);
            dynamic response = await _client.PostAsync(ENDPOINT_PREFIX, payload);

            return new ProductType(response);
        }

        /// <summary>
        /// Updates a product type.
        /// </summary>
        /// <param name="productType">Product type</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>ProductType</returns>
        public async Task<ProductType> UpdateProductTypeAsync(ProductType productType, List<JObject> actions)
        {
            return await UpdateProductTypeByIdAsync(productType.Id, productType.Version, actions);
        }

        /// <summary>
        /// Updates a product type by ID.
        /// </summary>
        /// <param name="productTypeId">ID of the product type</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype-by-id"/>
        public async Task<ProductType> UpdateProductTypeByIdAsync(string productTypeId, int version, List<JObject> actions, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(productTypeId))
            {
                throw new ArgumentException("Product type ID is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productTypeId);
            return await UpdateProductTypeAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Update a product type found by its Key.
        /// </summary>
        /// <param name="key">Product type key</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype-by-key"/>
        public async Task<ProductType> UpdateProductTypeByKeyAsync(string key, int version, List<JObject> actions)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Product key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return await UpdateProductTypeAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Private worker method for UpdateProductTypeByIdAsync and UpdateProductTypeByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype"/>
        private async Task<ProductType> UpdateProductTypeAsync(string endpoint, int version, List<JObject> actions)
        {
            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            if (actions == null || actions.Count < 1)
            {
                throw new ArgumentException("One or more update actions is required");
            }

            JObject data = JObject.FromObject(new
            {
                version = version,
                actions = new JArray(actions.ToArray())
            });

            dynamic response = await _client.PostAsync(endpoint, data.ToString());

            return new ProductType(response);
        }

        /// <summary>
        /// Deletes a ProductType.
        /// </summary>
        /// <param name="productType">ProductType</param>
        public async Task DeleteProductTypeAsync(ProductType productType)
        {
            await DeleteProductTypeByIdAsync(productType.Id, productType.Version);
        }

        /// <summary>
        /// Deletes a ProductType by its ID.
        /// </summary>
        /// <param name="productTypeId">ProductType ID</param>
        /// <param name="version">ProductType version</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#delete-producttype"/>
        public async Task DeleteProductTypeByIdAsync(string productTypeId, int version)
        {
            if (string.IsNullOrWhiteSpace(productTypeId))
            {
                throw new ArgumentException("productTypeId is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productTypeId);
            await _client.DeleteAsync(endpoint, values);
        }

        /// <summary>
        /// Deletes a ProductType by its key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="version">ProductType version</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#delete-producttype-by-key"/>
        public async Task DeleteProductTypeByKeyAsync(string key, int version)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", Helper.UrlEncode(key));
            await _client.DeleteAsync(endpoint, values);
        }

        #endregion
    }
}