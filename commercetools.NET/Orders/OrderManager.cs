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
        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("orderId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", orderId);
            dynamic response = await _client.GetAsync(endpoint);

            return new Order(response);
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
        public async Task<OrderQueryResult> QueryOrdersAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return new OrderQueryResult(response);
        }

        /// <summary>
        /// Creates an order from a Cart. The cart must have a shipping address set which is used for the tax calculation. The cart data is copied to the created order.
        /// </summary>
        /// <param name="draft">OrderFromCartDraft</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#create-order-from-cart"/>
        public async Task<Order> CreateOrderFromCartAsync(OrderFromCartDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentException("draft is required");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string payload = JsonConvert.SerializeObject(draft, settings);
            dynamic response = await _client.PostAsync(ENDPOINT_PREFIX, payload);

            return new Order(response);
        }

        /// <summary>
        /// Updates the OrderState.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="orderState">OrderState</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-orderstate"/>
        public async Task<Order> ChangeOrderStateAsync(Order order, OrderState orderState)
        {
            return await ChangeOrderStateAsync(order.Id, order.Version, orderState);
        }

        /// <summary>
        /// Updates the OrderState.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="orderState">OrderState</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-orderstate"/>
        public async Task<Order> ChangeOrderStateAsync(string orderId, int version, OrderState orderState)
        {
            JObject data = JObject.FromObject(new
            {
                action = "changeOrderState",
                orderState = orderState.ToString()
            });

            Order order = await UpdateOrderAsync(orderId, version, new List<JObject>() { data });

            return order;
        }

        /// <summary>
        /// Updates the ShipmentState.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="shipmentState">ShipmentState</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-shipmentstate"/>
        public async Task<Order> ChangeShipmentStateAsync(Order order, ShipmentState shipmentState)
        {
            return await ChangeShipmentStateAsync(order, shipmentState);
        }

        /// <summary>
        /// Updates the ShipmentState.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="shipmentState">ShipmentState</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-shipmentstate"/>
        public async Task<Order> ChangeShipmentStateAsync(string orderId, int version, ShipmentState shipmentState)
        {
            JObject data = JObject.FromObject(new
            {
                action = "changeShipmentState",
                shipmentState = shipmentState.ToString()
            });

            Order order = await UpdateOrderAsync(orderId, version, new List<JObject>() { data });

            return order;
        }

        /// <summary>
        /// Updates the PaymentState.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="paymentState">PaymentState</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-paymentstate"/>
        public async Task<Order> ChangePaymentStateAsync(Order order, PaymentState paymentState)
        {
            return await ChangePaymentStateAsync(order.Id, order.Version, paymentState);
        }

        /// <summary>
        /// Updates the PaymentState.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="paymentState">PaymentState</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#change-paymentstate"/>
        public async Task<Order> ChangePaymentStateAsync(string orderId, int version, PaymentState paymentState)
        {
            JObject data = JObject.FromObject(new
            {
                action = "changePaymentState",
                paymentState = paymentState.ToString()
            });

            Order order = await UpdateOrderAsync(orderId, version, new List<JObject>() { data });

            return order;
        }

        /// <summary>
        /// Update the SyncInfo.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="channel">Connection to particular synchronization destination.</param>
        /// <param name="externalId">Can be used to reference an external order instance, file etc.</param>
        /// <param name="syncedAt">If not provided, then current date would be used</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-syncinfo"/>
        public async Task<Order> UpdateSyncInfoAsync(Order order, Reference channel, string externalId = null, DateTime? syncedAt = null)
        {
            return await UpdateSyncInfoAsync(order.Id, order.Version, channel, externalId, syncedAt);
        }

        /// <summary>
        /// Update the SyncInfo.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="channel">Connection to particular synchronization destination.</param>
        /// <param name="externalId">Can be used to reference an external order instance, file etc.</param>
        /// <param name="syncedAt">If not provided, then current date would be used</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-syncinfo"/>
        public async Task<Order> UpdateSyncInfoAsync(string orderId, int version, Reference channel, string externalId = null, DateTime? syncedAt = null)
        {
            JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            JObject data = JObject.FromObject(new
            {
                action = "updateSyncInfo",
                channel = JObject.FromObject(channel, jsonSerializer)
            });

            if (!string.IsNullOrWhiteSpace(externalId))
            {
                data.Add(new JProperty("externalId", externalId));
            }

            if (syncedAt.HasValue)
            {
                data.Add(new JProperty("syncedAt", syncedAt.Value));
            }

            Order order = await UpdateOrderAsync(orderId, version, new List<JObject>() { data });

            return order;
        }

        /// <summary>
        /// This action adds a payment to the PaymentInfo. The payment must not be assigned to another Order or active Cart yet.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="payment">Payment</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#add-payment"/>
        public async Task<Order> AddPaymentAsync(Order order, Reference payment)
        {
            return await AddPaymentAsync(order.Id, order.Version, payment);
        }

        /// <summary>
        /// This action adds a payment to the PaymentInfo. The payment must not be assigned to another Order or active Cart yet.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="payment">Payment</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#add-payment"/>
        public async Task<Order> AddPaymentAsync(string orderId, int version, Reference payment)
        {
            JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            JObject data = JObject.FromObject(new
            {
                action = "addPayment",
                payment = JObject.FromObject(payment, jsonSerializer)
            });

            Order order = await UpdateOrderAsync(orderId, version, new List<JObject>() { data });

            return order;
        }

        /// <summary>
        /// This action removes a payment from the PaymentInfo.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="payment">Payment</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#remove-payment"/>
        public async Task<Order> RemovePaymentAsync(Order order, Reference payment)
        {
            return await RemovePaymentAsync(order.Id, order.Version, payment);
        }

        /// <summary>
        /// This action removes a payment from the PaymentInfo.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="payment">Payment</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#remove-payment"/>
        public async Task<Order> RemovePaymentAsync(string orderId, int version, Reference payment)
        {
            JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            JObject data = JObject.FromObject(new
            {
                action = "removePayment",
                payment = JObject.FromObject(payment, jsonSerializer)
            });

            Order order = await UpdateOrderAsync(orderId, version, new List<JObject>() { data });

            return order;
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="actions">The list of update actions to be performed on the order.</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-order"/>
        public async Task<Order> UpdateOrderAsync(Order order, List<JObject> actions)
        {
            return await UpdateOrderAsync(order.Id, order.Version, actions);
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <param name="version">The expected version of the order on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the order.</param>
        /// <returns>Order</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#update-order"/>
        public async Task<Order> UpdateOrderAsync(string orderId, int version, List<JObject> actions)
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
                actions = new JArray(actions.ToArray())
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", orderId);
            dynamic response = await _client.PostAsync(endpoint, data.ToString());

            return new Order(response);
        }

        /// <summary>
        /// Removes an Order. Only orders created for testing should be deleted.
        /// </summary>
        /// <param name="order">Order</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#delete-order"/>
        public async Task DeleteOrderAsync(Order order)
        {
            await DeleteOrderAsync(order.Id, order.Version);
        }

        /// <summary>
        /// Removes an Order. Only orders created for testing should be deleted.
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="version">Order version</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#delete-order"/>
        public async Task DeleteOrderAsync(string orderId, int version)
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
            await _client.DeleteAsync(endpoint, values);
        }

        #endregion
    }
}