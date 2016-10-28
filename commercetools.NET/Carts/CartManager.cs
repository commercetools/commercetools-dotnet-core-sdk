using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Customers;
using commercetools.CustomFields;
using commercetools.Products;

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
        /// <param name="configuration"></param>
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
        public async Task<Cart> GetCartByIdAsync(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("cartCId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", cartId);
            dynamic response = await _client.GetAsync(endpoint);

            return new Cart(response);
        }

        /// <summary>
        /// Retrieves the active cart of the customer that has been modified most recently.
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#get-cart-by-customer-id"/>
        public async Task<Cart> GetCartByCustomerAsync(Customer customer)
        {
            return await GetCartByCustomerIdAsync(customer.Id);
        }

        /// <summary>
        /// Retrieves the active cart of the customer that has been modified most recently.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#get-cart-by-customer-id"/>
        public async Task<Cart> GetCartByCustomerIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("customerId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/?customerId=", customerId);
            dynamic response = await _client.GetAsync(endpoint);

            return new Cart(response);
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
        public async Task<CartQueryResult> QueryCartsAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return new CartQueryResult(response);
        }

        /// <summary>
        /// Creates a new Cart.
        /// </summary>
        /// <param name="cartDraft">CartDraft</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#create-cart"/>
        public async Task<Cart> CreateCartAsync(CartDraft cartDraft)
        {
            if (string.IsNullOrWhiteSpace(cartDraft.Currency))
            {
                throw new ArgumentException("currency is required");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string payload = JsonConvert.SerializeObject(cartDraft, settings);
            dynamic response = await _client.PostAsync(ENDPOINT_PREFIX, payload);

            return new Cart(response);
        }

        /// <summary>
        /// Adds a product variant in the given quantity to the cart. If the cart already contains the product variant for the given supply and distribution channel, then only quantity of the LineItem is increased.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="product">An existing Product</param>
        /// <param name="productVariant">An existing ProductVariant in the product</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="supplyChannel">By providing supply channel information, you can unique identify inventory entries that should be reserved. Provided channel should have the role InventorySupply.</param>
        /// <param name="distributionChannel">The channel is used to select a ProductPrice. Provided channel should have the role ProductDistribution.</param>
        /// <param name="custom">The custom fields.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#add-lineitem"/>
        public async Task<Cart> AddLineItemAsync(Cart cart, Product product, ProductVariant productVariant, int quantity = 1, Reference supplyChannel = null, Reference distributionChannel = null, CustomFieldsDraft custom = null)
        {
            return await AddLineItemAsync(cart.Id, cart.Version, product.Id, productVariant.Id, quantity, supplyChannel, distributionChannel, custom);
        }

        /// <summary>
        /// Adds a product variant in the given quantity to the cart. If the cart already contains the product variant for the given supply and distribution channel, then only quantity of the LineItem is increased.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="productId">ID of an existing Product</param>
        /// <param name="variantId">ID of an existing ProductVariant in the product</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="supplyChannel">By providing supply channel information, you can unique identify inventory entries that should be reserved. Provided channel should have the role InventorySupply.</param>
        /// <param name="distributionChannel">The channel is used to select a ProductPrice. Provided channel should have the role ProductDistribution.</param>
        /// <param name="custom">The custom fields.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#add-lineitem"/>
        public async Task<Cart> AddLineItemAsync(string cartId, int version, string productId, int variantId, int quantity = 1, Reference supplyChannel = null, Reference distributionChannel = null, CustomFieldsDraft custom = null)
        {
            JObject data = JObject.FromObject(new
            {
                action = "addLineItem",
                productId = productId,
                variantId = variantId,
                quantity = quantity
            });

            JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            if (supplyChannel != null)
            {
                data.Add(new JProperty("supplyChannel", JObject.FromObject(supplyChannel, jsonSerializer)));
            }

            if (distributionChannel != null)
            {
                data.Add(new JProperty("distributionChannel", JObject.FromObject(distributionChannel, jsonSerializer)));
            }

            if (custom != null)
            {
                data.Add(new JProperty("custom", JObject.FromObject(custom, jsonSerializer)));
            }

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Decreases the quantity of the given LineItem. If after the update the quantity of the line item is not greater than 0 or the quantity is not specified, the line item is removed from the cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="lineItemId">Id of an existing LineItem in the cart.</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#remove-lineitem"/>
        public async Task<Cart> RemoveLineItemAsync(Cart cart, string lineItemId, int quantity = 0)
        {
            return await RemoveLineItemAsync(cart.Id, cart.Version, lineItemId, quantity);
        }

        /// <summary>
        /// Decreases the quantity of the given LineItem. If after the update the quantity of the line item is not greater than 0 or the quantity is not specified, the line item is removed from the cart.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="lineItemId">Id of an existing LineItem in the cart.</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#remove-lineitem"/>
        public async Task<Cart> RemoveLineItemAsync(string cartId, int version, string lineItemId, int quantity = 0)
        {
            JObject data = JObject.FromObject(new
            {
                action = "removeLineItem",
                lineItemId = lineItemId
            });

            if (quantity > 0)
            {
                data.Add(new JProperty("quantity", quantity));
            }

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Sets the quantity of the given LineItem. If quantity is 0, line item is removed from the cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="lineItemId">Id of an existing LineItem in the cart.</param>
        /// <param name="quantity">New quantity for the LineItem</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#change-lineitem-quantity"/>
        public async Task<Cart> ChangeLineItemQuantityAsync(Cart cart, string lineItemId, int quantity)
        {
            return await ChangeLineItemQuantityAsync(cart.Id, cart.Version, lineItemId, quantity);
        }

        /// <summary>
        /// Sets the quantity of the given LineItem. If quantity is 0, line item is removed from the cart.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="lineItemId">Id of an existing LineItem in the cart.</param>
        /// <param name="quantity">New quantity for the LineItem</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#change-lineitem-quantity"/>
        public async Task<Cart> ChangeLineItemQuantityAsync(string cartId, int version, string lineItemId, int quantity)
        {
            JObject data = JObject.FromObject(new
            {
                action = "changeLineItemQuantity",
                lineItemId = lineItemId,
                quantity = quantity
            });

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Sets the shipping address of the cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="address">Shipping address</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-shipping-address"/>
        public async Task<Cart> SetShippingAddressAsync(Cart cart, Address address = null)
        {
            return await SetShippingAddressAsync(cart.Id, cart.Version, address);
        }

        /// <summary>
        /// Sets the shipping address of the cart.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="address">Shipping address</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-shipping-address"/>
        public async Task<Cart> SetShippingAddressAsync(string cartId, int version, Address address = null)
        {
            JObject data = JObject.FromObject(new
            {
                action = "setShippingAddress"
            });

            if (address != null)
            {
                JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
                data.Add(new JProperty("address", JObject.FromObject(address, jsonSerializer)));
            }

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Sets the billing address of the cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="address">Billing address</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-billing-address"/>
        public async Task<Cart> SetBillingAddressAsync(Cart cart, Address address = null)
        {
            return await SetBillingAddressAsync(cart.Id, cart.Version, address);
        }

        /// <summary>
        /// Sets the billing address of the cart.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="address">Billing address</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-billing-address"/>
        public async Task<Cart> SetBillingAddressAsync(string cartId, int version, Address address = null)
        {
            JObject data = JObject.FromObject(new
            {
                action = "setBillingAddress"
            });

            if (address != null)
            {
                JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
                data.Add(new JProperty("address", JObject.FromObject(address, jsonSerializer)));
            }

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Sets the ShippingMethod. Prerequisite: at least the country for the shipping address must have been set beforehand.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="shippingMethod"></param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-shippingmethod"/>
        public async Task<Cart> SetShippingMethodAsync(Cart cart, Reference shippingMethod = null)
        {
            return await SetShippingMethodAsync(cart.Id, cart.Version, shippingMethod);
        }

        /// <summary>
        /// Sets the ShippingMethod. Prerequisite: at least the country for the shipping address must have been set beforehand.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="shippingMethod"></param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-shippingmethod"/>
        public async Task<Cart> SetShippingMethodAsync(string cartId, int version, Reference shippingMethod = null)
        {
            JObject data = JObject.FromObject(new
            {
                action = "setShippingMethod"
            });

            if (shippingMethod != null)
            {
                JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
                data.Add(new JProperty("shippingMethod", JObject.FromObject(shippingMethod, jsonSerializer)));
            }

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Sets the customer ID of the cart. When the customer ID is set, the LineItem prices are updated.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="customerId">If set, a customer with the given ID must exist in the project.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-customer-id"/>
        public async Task<Cart> SetCustomerIdAsync(Cart cart, string customerId = null)
        {
            return await SetCustomerIdAsync(cart.Id, cart.Version, customerId);
        }

        /// <summary>
        /// Sets the customer ID of the cart. When the customer ID is set, the LineItem prices are updated.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="customerId">If set, a customer with the given ID must exist in the project.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#set-customer-id"/>
        public async Task<Cart> SetCustomerIdAsync(string cartId, int version, string customerId = null)
        {
            JObject data = JObject.FromObject(new
            {
                action = "setCustomerId"
            });

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                data.Add(new JProperty("customerId", customerId));
            }

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Updates tax rates and prices.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#recalculate"/>
        public async Task<Cart> RecalculateAsync(Cart cart)
        {
            return await RecalculateAsync(cart.Id, cart.Version);
        }

        /// <summary>
        /// Updates tax rates and prices.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#recalculate"/>
        public async Task<Cart> RecalculateAsync(string cartId, int version)
        {
            JObject data = JObject.FromObject(new
            {
                action = "recalculate"
            });

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// This action adds a payment to the PaymentInfo. The payment must not be assigned to another Order or active Cart yet.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="payment">Reference to a Payment</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#add-payment"/>
        public async Task<Cart> AddPaymentAsync(Cart cart, Reference payment)
        {
            return await AddPaymentAsync(cart.Id, cart.Version, payment);
        }

        /// <summary>
        /// This action adds a payment to the PaymentInfo. The payment must not be assigned to another Order or active Cart yet.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="payment">Reference to a Payment</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#add-payment"/>
        public async Task<Cart> AddPaymentAsync(string cartId, int version, Reference payment)
        {
            JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            JObject data = JObject.FromObject(new
            {
                action = "addPayment",
                payment = JObject.FromObject(payment, jsonSerializer)
            });

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// This action removes a payment from the PaymentInfo.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="payment">Reference to a Payment</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#remove-payment"/>
        public async Task<Cart> RemovePaymentAsync(Cart cart, Reference payment)
        {
            return await RemovePaymentAsync(cart.Id, cart.Version, payment);
        }

        /// <summary>
        /// This action removes a payment from the PaymentInfo.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="payment">Reference to a Payment</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#remove-payment"/>
        public async Task<Cart> RemovePaymentAsync(string cartId, int version, Reference payment)
        {
            JsonSerializer jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            JObject data = JObject.FromObject(new
            {
                action = "removePayment",
                payment = JObject.FromObject(payment, jsonSerializer)
            });

            return await UpdateCartAsync(cartId, version, new List<JObject>() { data });
        }

        /// <summary>
        /// Updates a cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="actions">The list of update actions to be performed on the cart.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#update-cart"/>
        public async Task<Cart> UpdateCartAsync(Cart cart, List<JObject> actions)
        {
            return await UpdateCartAsync(cart.Id, cart.Version, actions);
        }

        /// <summary>
        /// Updates a cart.
        /// </summary>
        /// <param name="cartId">ID of the cart</param>
        /// <param name="version">The expected version of the cart on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the cart.</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#update-cart"/>
        public async Task<Cart> UpdateCartAsync(string cartId, int version, List<JObject> actions)
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
                actions = new JArray(actions.ToArray())
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", cartId);
            dynamic response = await _client.PostAsync(endpoint, data.ToString());

            return new Cart(response);
        }

        /// <summary>
        /// Removes a Cart. If it was assigned to a Customer, a new cart can be created for this customer.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#delete-cart"/>
        public async Task<Cart> DeleteCartAsync(Cart cart)
        {
            return await DeleteCartAsync(cart.Id, cart.Version);
        }

        /// <summary>
        /// Removes a Cart. If it was assigned to a Customer, a new cart can be created for this customer.
        /// </summary>
        /// <param name="cartId">Cart ID</param>
        /// <param name="version">Cart version</param>
        /// <returns>Cart</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#delete-cart"/>
        public async Task<Cart> DeleteCartAsync(string cartId, int version)
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
            dynamic response = await _client.DeleteAsync(endpoint, values);

            return new Cart(response);
        }

        #endregion
    }
}