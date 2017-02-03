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
        public Task<Response<ProductType>> GetProductTypeByIdAsync(string productTypeId)
        {
            if (string.IsNullOrWhiteSpace(productTypeId))
            {
                throw new ArgumentException("productTypeId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productTypeId);
            return _client.GetAsync<ProductType>(endpoint);
        }

        /// <summary>
        /// Gets a ProductType by its key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#get-a-producttype-by-key"/>
        /// <returns>ProductType</returns>
        public Task<Response<ProductType>> GetProductTypeByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", Helper.UrlEncode(key));
            return _client.GetAsync<ProductType>(endpoint);
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
        public Task<Response<ProductTypeQueryResult>> QueryProductTypesAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<ProductTypeQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new ProductType.
        /// </summary>
        /// <param name="productTypeDraft">ProductTypeDraft</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#create-a-producttype"/>
        public Task<Response<ProductType>> CreateProductTypeAsync(ProductTypeDraft productTypeDraft)
        {
            if (string.IsNullOrWhiteSpace(productTypeDraft.Name))
            {
                throw new ArgumentException("name is required");
            }

            if (string.IsNullOrWhiteSpace(productTypeDraft.Description))
            {
                throw new ArgumentException("description is required");
            }

            string payload = JsonConvert.SerializeObject(productTypeDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<ProductType>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a product type.
        /// </summary>
        /// <param name="productType">Product type</param>
        /// <param name="action">The update action to apply to the product type.</param>
        /// <returns>ProductType</returns>
        public Task<Response<ProductType>> UpdateProductTypeAsync(ProductType productType, UpdateAction action)
        {
            return UpdateProductTypeByIdAsync(productType.Id, productType.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a product type.
        /// </summary>
        /// <param name="productType">Product type</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>ProductType</returns>
        public Task<Response<ProductType>> UpdateProductTypeAsync(ProductType productType, List<UpdateAction> actions)
        {
            return UpdateProductTypeByIdAsync(productType.Id, productType.Version, actions);
        }

        /// <summary>
        /// Updates a product type by ID.
        /// </summary>
        /// <param name="productTypeId">ID of the product type</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="action">The update action to apply to the product type.</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype-by-id"/>
        public Task<Response<ProductType>> UpdateProductTypeByIdAsync(string productTypeId, int version, UpdateAction action)
        {
            return UpdateProductTypeByIdAsync(productTypeId, version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a product type by ID.
        /// </summary>
        /// <param name="productTypeId">ID of the product type</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype-by-id"/>
        public Task<Response<ProductType>> UpdateProductTypeByIdAsync(string productTypeId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(productTypeId))
            {
                throw new ArgumentException("Product type ID is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productTypeId);
            return UpdateProductTypeAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Update a product type found by its Key.
        /// </summary>
        /// <param name="key">Product type key</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="action">The action to apply to the product type.</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype-by-key"/>
        public Task<Response<ProductType>> UpdateProductTypeByKeyAsync(string key, int version, UpdateAction action)
        {
            return UpdateProductTypeByKeyAsync(key, version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Update a product type found by its Key.
        /// </summary>
        /// <param name="key">Product type key</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>ProductType</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype-by-key"/>
        public Task<Response<ProductType>> UpdateProductTypeByKeyAsync(string key, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Product key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return UpdateProductTypeAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Private worker method for UpdateProductTypeByIdAsync and UpdateProductTypeByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">The expected version of the product type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product type.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#update-producttype"/>
        private Task<Response<ProductType>> UpdateProductTypeAsync(string endpoint, int version, List<UpdateAction> actions)
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
                actions = JArray.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore })
            });

            return _client.PostAsync<ProductType>(endpoint, data.ToString());
        }

        /// <summary>
        /// Deletes a ProductType.
        /// </summary>
        /// <param name="productType">ProductType</param>
        public Task<Response<JObject>> DeleteProductTypeAsync(ProductType productType)
        {
            return DeleteProductTypeByIdAsync(productType.Id, productType.Version);
        }

        /// <summary>
        /// Deletes a ProductType by its ID.
        /// </summary>
        /// <param name="productTypeId">ProductType ID</param>
        /// <param name="version">ProductType version</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#delete-producttype"/>
        public Task<Response<JObject>> DeleteProductTypeByIdAsync(string productTypeId, int version)
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
            return _client.DeleteAsync<JObject>(endpoint, values);
        }

        /// <summary>
        /// Deletes a ProductType by its key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="version">ProductType version</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#delete-producttype-by-key"/>
        public Task<Response<JObject>> DeleteProductTypeByKeyAsync(string key, int version)
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
            return _client.DeleteAsync<JObject>(endpoint, values);
        }

        #endregion
    }
}
