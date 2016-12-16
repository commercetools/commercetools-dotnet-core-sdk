using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Products
{
    /// <summary>
    /// Provides access to the functions in the Products section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html"/>
    public class ProductManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/products";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public ProductManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets the full representation of a product by ID.
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#get-product-by-id"/>
        public async Task<Response<Product>> GetProductByIdAsync(string productId, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("productId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productId);
            return await GetProductAsync(endpoint, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// Gets the full representation of a product by Key.
        /// </summary>
        /// <param name="key">Product key</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#get-product-by-key"/>
        public async Task<Response<Product>> GetProductByKeyAsync(string key, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return await GetProductAsync(endpoint, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// Private worker method for UpdateProductByIdAsync and UpdateProductByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#get-product"/>
        private async Task<Response<Product>> GetProductAsync(string endpoint, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            NameValueCollection values = new NameValueCollection();

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

            return await _client.GetAsync<Product>(endpoint, values);
        }

        /// <summary>
        /// Queries for Products.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <param name="priceCurrency">Enables price selection</param>
        /// <param name="priceCountry">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter</param>
        /// <returns>ProductQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#products-by-query"/>
        public async Task<Response<ProductQueryResult>> QueryProductsAsync(
            string where = null, 
            string sort = null, 
            int limit = -1, 
            int offset = -1,
            string priceCurrency = null,
            string priceCountry = null,
            string priceCustomerGroup = null,
            string priceChannel = null)
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

            return await _client.GetAsync<ProductQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new Product.
        /// </summary>
        /// <param name="productDraft">ProductDraft</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#create-product"/>
        public async Task<Response<Product>> CreateProductAsync(ProductDraft productDraft)
        {
            if (productDraft.Name == null || productDraft.Name.IsEmpty())
            {
                throw new ArgumentException("name is required");
            }

            if (productDraft.ProductType == null)
            {
                throw new ArgumentException("productType is required");
            }

            if (productDraft.Slug == null || productDraft.Slug.IsEmpty())
            {
                throw new ArgumentException("slug is required");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string payload = JsonConvert.SerializeObject(productDraft, settings);
            return await _client.PostAsync<Product>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// (Partial) updates are made to an existing product by sending a list of actions to be applied. The actions are applied in the given order. If price selection query parameters are provided, the selected prices will be added to the response.
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="actions">The list of update actions to apply to the product.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#update-product"/>
        public async Task<Response<Product>> UpdateProductAsync(Product product, List<UpdateAction> actions, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            return await UpdateProductByIdAsync(product.Id, product.Version, actions, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// (Partial) updates are made to an existing product by sending a list of actions to be applied. The actions are applied in the given order. If price selection query parameters are provided, the selected prices will be added to the response.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="version">The expected version of the product on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#update-product"/>
        public async Task<Response<Product>> UpdateProductByIdAsync(string productId, int version, List<UpdateAction> actions, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productId);
            return await UpdateProductAsync(endpoint, version, actions, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// Update a product found by its Key.
        /// </summary>
        /// <param name="key">Product key</param>
        /// <param name="version">The expected version of the product on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#update-product"/>
        public async Task<Response<Product>> UpdateProductByKeyAsync(string key, int version, List<UpdateAction> actions, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Product key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return await UpdateProductAsync(endpoint, version, actions, priceCurrency, priceCountry, priceCustomerGroup, priceChannel);
        }

        /// <summary>
        /// Private worker method for UpdateProductByIdAsync and UpdateProductByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">The expected version of the product on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product.</param>
        /// <param name="priceCurrency">The currency code compliant to ISO 4217. Enables price selection.</param>
        /// <param name="priceCountry">A two-digit country code as per ISO 3166-1 alpha-2. Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceCustomerGroup">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <param name="priceChannel">Enables price selection. Can only be used in conjunction with the priceCurrency parameter.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#update-product"/>
        private async Task<Response<Product>> UpdateProductAsync(string endpoint, int version, List<UpdateAction> actions, string priceCurrency = null, string priceCountry = null, Guid priceCustomerGroup = new Guid(), Guid priceChannel = new Guid())
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

            if (!string.IsNullOrWhiteSpace(priceCurrency))
            {
                data.Add(new JProperty("priceCurrency", priceCurrency));

                if (!string.IsNullOrWhiteSpace(priceCountry))
                {
                    data.Add(new JProperty("priceCountry", priceCountry));
                }

                if (priceCustomerGroup != Guid.Empty)
                {
                    data.Add(new JProperty("priceCustomerGroup", priceCustomerGroup));
                }

                if (priceChannel != Guid.Empty)
                {
                    data.Add(new JProperty("priceChannel", priceChannel));
                }
            }

            return await _client.PostAsync<Product>(endpoint, data.ToString());
        }

        /// <summary>
        /// Deletes a Product.
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#delete-product"/>
        public async Task<Response<Product>> DeleteProductAsync(Product product)
        {
            return await DeleteProductAsync(product.Id, product.Version);
        }

        /// <summary>
        /// Deletes a Product.
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="version">Product version</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-products.html#delete-product"/>
        public async Task<Response<Product>> DeleteProductAsync(string productId, int version)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", productId);
            return await _client.DeleteAsync<Product>(endpoint, values);
        }

        #endregion
    }
}
