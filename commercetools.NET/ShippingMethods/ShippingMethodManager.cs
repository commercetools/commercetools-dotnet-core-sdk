using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.ShippingMethods
{
    /// <summary>
    /// Provides access to the functions in the ShippingMethods section of the API.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html"/>
    public class ShippingMethodManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/shipping-methods";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public ShippingMethodManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a ShippingMethod by its ID.
        /// </summary>
        /// <param name="shippingMethodId">ShippingMethod ID</param>
        /// <returns>ShippingMethod</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#get-shippingmethod-by-id"/>
        public Task<Response<ShippingMethod>> GetShippingMethodByIdAsync(string shippingMethodId)
        {
            if (string.IsNullOrWhiteSpace(shippingMethodId))
            {
                throw new ArgumentException("shippingMethodId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", shippingMethodId);
            return _client.GetAsync<ShippingMethod>(endpoint);
        }

        /// <summary>
        /// Queries for ShippingMethods.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>ShippingMethodQueryResult</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#query-shippingmethods"/>
        public Task<Response<ShippingMethodQueryResult>> QueryShippingMethodsAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<ShippingMethodQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new Shipping Method.
        /// </summary>
        /// <param name="shippingMethodDraft">ShippingMethodDraft</param>
        /// <returns>ShippingMethod</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-shippingMethods.html#create-shippingmethod"/>
        public Task<Response<ShippingMethod>> CreateShippingMethodAsync(ShippingMethodDraft shippingMethodDraft)
        {
            if (string.IsNullOrWhiteSpace(shippingMethodDraft.Name))
            {
                throw new ArgumentException("name is required");
            }

            string payload = JsonConvert.SerializeObject(shippingMethodDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<ShippingMethod>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a shipping method.
        /// </summary>
        /// <param name="shippingMethod">Shipping method</param>
        /// <param name="actions">The list of update actions to apply to the product.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-shippingMethods.html#update-shippingmethod"/>
        public Task<Response<ShippingMethod>> UpdateShippingMethodAsync(ShippingMethod shippingMethod, List<UpdateAction> actions)
        {
            return UpdateShippingMethodAsync(shippingMethod.Id, shippingMethod.Version, actions);
        }

        /// <summary>
        /// Updates a shipping method.
        /// </summary>
        /// <param name="shippingMethodId">ID of the shipping method</param>
        /// <param name="version">The expected version of the product on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the product.</param>
        /// <returns>Product</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-shippingMethods.html#update-shippingmethod"/>
        public Task<Response<ShippingMethod>> UpdateShippingMethodAsync(string shippingMethodId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(shippingMethodId))
            {
                throw new ArgumentException("Shipping method ID is required");
            }

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

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", shippingMethodId);
            return _client.PostAsync<ShippingMethod>(endpoint, data.ToString());
        }

        /// <summary>
        /// Removes a shipping method.
        /// </summary>
        /// <param name="shippingMethod">Shipping method</param>
        /// <returns>ShippingMethod</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-shippingMethods.html#delete-shippingmethod"/>
        public Task<Response<ShippingMethod>> DeleteShippingMethodAsync(ShippingMethod shippingMethod)
        {
            return DeleteShippingMethodAsync(shippingMethod.Id, shippingMethod.Version);
        }

        /// <summary>
        /// Removes a shipping method.
        /// </summary>
        /// <param name="shippingMethodId">Shipping method ID</param>
        /// <param name="version">Shipping method version</param>
        /// <returns>ShippingMethod</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-shippingMethods.html#delete-shippingmethod"/>
        public Task<Response<ShippingMethod>> DeleteShippingMethodAsync(string shippingMethodId, int version)
        {
            if (string.IsNullOrWhiteSpace(shippingMethodId))
            {
                throw new ArgumentException("Shipping method ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", shippingMethodId);
            return _client.DeleteAsync<ShippingMethod>(endpoint, values);
        }

        #endregion
    }
}
