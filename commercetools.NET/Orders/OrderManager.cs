using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Orders
{
    /// <summary>
    /// Provides access to the functions in the Orders section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html"/>
    public class OrderManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/orders";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public OrderManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets an Order by its ID.
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#get-order-by-id"/>
        public Task<Response<Order>> GetOrderByIdAsync(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("orderId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", orderId);
            return _client.GetAsync<Order>(endpoint);
        }

        /// <summary>
        /// Queries for Orders.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>OrderQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#query-orders"/>
        public Task<Response<OrderQueryResult>> QueryOrdersAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<OrderQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates an order from a Cart. The cart must have a shipping address set which is used for the tax calculation. The cart data is copied to the created order.
        /// </summary>
        /// <param name="draft">OrderFromCartDraft</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#create-order-from-cart"/>
        public Task<Response<Order>> CreateOrderFromCartAsync(OrderFromCartDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentException("draft is required");
            }

            string payload = JsonConvert.SerializeObject(draft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<Order>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="action">The update action to be performed on the order.</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-order"/>
        public Task<Response<Order>> UpdateOrderAsync(Order order, UpdateAction action)
        {
            return UpdateOrderAsync(order.Id, order.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="actions">The list of update actions to be performed on the order.</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-order"/>
        public Task<Response<Order>> UpdateOrderAsync(Order order, List<UpdateAction> actions)
        {
            return UpdateOrderAsync(order.Id, order.Version, actions);
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the order.</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-order"/>
        public Task<Response<Order>> UpdateOrderAsync(string orderId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("orderId is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("version is required");
            }

            if (actions == null || actions.Count < 1)
            {
                throw new ArgumentException("One or more actions is required");
            }

            JObject data = JObject.FromObject(new
            {
                version = version,
                actions = JArray.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore })
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", orderId);
            return _client.PostAsync<Order>(endpoint, data.ToString());
        }

        /// <summary>
        /// Removes an Order. Only orders created for testing should be deleted.
        /// </summary>
        /// <param name="order">Order</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#delete-order"/>
        public Task<Response<JObject>> DeleteOrderAsync(Order order)
        {
            return DeleteOrderAsync(order.Id, order.Version);
        }

        /// <summary>
        /// Removes an Order. Only orders created for testing should be deleted.
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="version">Order version</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#delete-order"/>
        public Task<Response<JObject>> DeleteOrderAsync(string orderId, int version)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("orderId is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", orderId);
            return _client.DeleteAsync<JObject>(endpoint, values);
        }

        #endregion
    }
}
