using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Customers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Carts
{
    /// <summary>
    /// Provides access to the functions in the Carts section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html"/>
    public class CartManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/carts";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public CartManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a Cart by its ID.
        /// </summary>
        /// <param name="cartId">Cart ID</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#get-cart-by-id"/>
        /// <returns>Cart</returns>
        public Task<Response<Cart>> GetCartByIdAsync(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("cartId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", cartId);
            return _client.GetAsync<Cart>(endpoint);
        }

        /// <summary>
        /// Retrieves the active cart of the customer that has been modified most recently.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#get-cart-by-customer-id"/>
        public Task<Response<Cart>> GetCartByCustomerAsync(Customer customer)
        {
            return GetCartByCustomerIdAsync(customer.Id);
        }

        /// <summary>
        /// Retrieves the active cart of the customer that has been modified most recently.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#get-cart-by-customer-id"/>
        public Task<Response<Cart>> GetCartByCustomerIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("customerId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/?customerId=", customerId);
            return _client.GetAsync<Cart>(endpoint);
        }

        /// <summary>
        /// Queries for Carts.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>CartQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#query-carts"/>
        public Task<Response<CartQueryResult>> QueryCartsAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<CartQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new Cart.
        /// </summary>
        /// <param name="cartDraft">CartDraft</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#create-cart"/>
        public Task<Response<Cart>> CreateCartAsync(CartDraft cartDraft)
        {
            if (string.IsNullOrWhiteSpace(cartDraft.Currency))
            {
                throw new ArgumentException("currency is required");
            }

            string payload = JsonConvert.SerializeObject(cartDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<Cart>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="action">The update action to be performed on the cart.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#update-cart"/>
        public Task<Response<Cart>> UpdateCartAsync(Cart cart, UpdateAction action)
        {
            return UpdateCartAsync(cart.Id, cart.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="actions">The list of update actions to be performed on the cart.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#update-cart"/>
        public Task<Response<Cart>> UpdateCartAsync(Cart cart, List<UpdateAction> actions)
        {
            return UpdateCartAsync(cart.Id, cart.Version, actions);
        }

        /// <summary>
        /// Updates a cart.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the cart.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#update-cart"/>
        public Task<Response<Cart>> UpdateCartAsync(string cartId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("Cart ID is required");
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

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", cartId);
            return _client.PostAsync<Cart>(endpoint, data.ToString());
        }

        /// <summary>
        /// Removes a Cart. If it was assigned to a Customer, a new cart can be created for this customer.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#delete-cart"/>
        public Task<Response<Cart>> DeleteCartAsync(Cart cart)
        {
            return DeleteCartAsync(cart.Id, cart.Version);
        }

        /// <summary>
        /// Removes a Cart. If it was assigned to a Customer, a new cart can be created for this customer.
        /// </summary>
        /// <param name="cartId">Cart ID</param>
        /// <param name="version">Cart version</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#delete-cart"/>
        public Task<Response<Cart>> DeleteCartAsync(string cartId, int version)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("Cart ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", cartId);
            return _client.DeleteAsync<Cart>(endpoint, values);
        }

        #endregion
    }
}
