using System;
using System.Collections.Generic;

using commercetools.Carts;
using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Orders
{
    /// <summary>
    /// An order can be created from a cart, usually after a checkout process has been completed. Orders can also be imported.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#order"/>
    public class Order
    {
        #region Properties

        /// <summary>
        /// The unique ID of the order.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The current version of the order.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        /// <summary>
        /// Created At
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        /// Last Modified At
        /// </summary>
        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        /// <summary>
        /// This field will only be present if it was set for Order Import
        /// </summary>
        [JsonProperty(PropertyName = "completedAt")]
        public DateTime? CompletedAt { get; private set; }

        /// <summary>
        /// String that uniquely identifies an order.
        /// </summary>
        /// <remarks>
        /// It can be used to create more human-readable (in contrast to ID) identifier for the order. It should be unique across a project. Once it’s set it cannot be changed.
        /// </remarks>
        [JsonProperty(PropertyName = "orderNumber")]
        public string OrderNumber { get; private set; }

        /// <summary>
        /// Customer Id
        /// </summary>
        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; private set; }

        /// <summary>
        /// Customer Email
        /// </summary>
        [JsonProperty(PropertyName = "customerEmail")]
        public string CustomerEmail { get; private set; }

        /// <summary>
        /// Identifies carts and orders belonging to an anonymous session (the customer has not signed up/in yet).
        /// </summary>
        [JsonProperty(PropertyName = "anonymousId")]
        public string AnonymousId { get; private set; }

        /// <summary>
        /// List of LineItems
        /// </summary>
        [JsonProperty(PropertyName = "lineItems")]
        public List<LineItem> LineItems { get; private set; }

        /// <summary>
        /// List of CustomLineItems
        /// </summary>
        [JsonProperty(PropertyName = "customLineItems")]
        public List<CustomLineItem> CustomLineItems { get; private set; }

        /// <summary>
        /// Total Price
        /// </summary>
        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; private set; }

        /// <summary>
        /// The taxes are calculated based on the shipping address.
        /// </summary>
        [JsonProperty(PropertyName = "taxedPrice")]
        public TaxedPrice TaxedPrice { get; private set; }

        /// <summary>
        /// Shipping Address
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddress")]
        public Address ShippingAddress { get; private set; }

        /// <summary>
        /// Billing Address
        /// </summary>
        [JsonProperty(PropertyName = "billingAddress")]
        public Address BillingAddress { get; private set; }

        /// <summary>
        /// Tax Mode
        /// </summary>
        [JsonProperty(PropertyName = "taxMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaxMode? TaxMode { get; private set; }

        /// <summary>
        /// When calculating taxes for taxedPrice, the selected mode is used for rouding.
        /// </summary>
        [JsonProperty(PropertyName = "taxRoundingMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoundingMode? TaxRoundingMode { get; private set; }

        /// <summary>
        /// Set when the customer is set and the customer is a member of a customer group. Used for product variant price selection.
        /// </summary>
        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; private set; }

        /// <summary>
        /// A two-digit country code as per ISO 3166-1 alpha-2. Used for product variant price selection.
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; private set; }

        /// <summary>
        /// One of the four predefined OrderStates.
        /// </summary>
        [JsonProperty(PropertyName = "orderState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderState? OrderState { get; private set; }

        /// <summary>
        /// This reference can point to a state in a custom workflow.
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public Reference State { get; private set; }

        /// <summary>
        /// Shipment State
        /// </summary>
        [JsonProperty(PropertyName = "shipmentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipmentState? ShipmentState { get; private set; }

        /// <summary>
        /// Payment State
        /// </summary>
        [JsonProperty(PropertyName = "paymentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentState? PaymentState { get; private set; }

        /// <summary>
        /// Shipping Info
        /// </summary>
        [JsonProperty(PropertyName = "shippingInfo")]
        public ShippingInfo ShippingInfo { get; private set; }

        /// <summary>
        /// List of SyncInfo
        /// </summary>
        [JsonProperty(PropertyName = "syncInfo")]
        public List<SyncInfo> SyncInfo { get; private set; }

        /// <summary>
        /// List of ReturnInfo
        /// </summary>
        [JsonProperty(PropertyName = "returnInfo")]
        public List<ReturnInfo> ReturnInfo { get; private set; }

        /// <summary>
        /// List of DiscountCodeInfo
        /// </summary>
        [JsonProperty(PropertyName = "discountCodes")]
        public List<DiscountCodeInfo> DiscountCodes { get; private set; }

        /// <summary>
        /// The sequence number of the last order message produced by changes to this order. 0 means, that no messages were created yet.
        /// </summary>
        [JsonProperty(PropertyName = "lastMessageSequenceNumber")]
        public int? LastMessageSequenceNumber { get; private set; }

        /// <summary>
        /// Set when this order was created from a cart. The cart will have the state Ordered.
        /// </summary>
        [JsonProperty(PropertyName = "cart")]
        public Reference Cart { get; private set; }

        /// <summary>
        /// Custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        /// <summary>
        /// Payment Info
        /// </summary>
        [JsonProperty(PropertyName = "paymentInfo")]
        public PaymentInfo PaymentInfo { get; private set; }

        /// <summary>
        /// String conforming to IETF language tag.
        /// </summary>
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; private set; }

        /// <summary>
        /// Inventory Mode
        /// </summary>
        [JsonProperty(PropertyName = "inventoryMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InventoryMode? InventoryMode { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Order(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            TaxMode taxMode;
            RoundingMode taxRoundingMode;
            OrderState orderState;
            ShipmentState shipmentState;
            PaymentState paymentState;
            InventoryMode inventoryMode;

            string taxModeStr = (data.taxMode != null ? data.taxMode.ToString() : string.Empty);
            string taxRoundingModeStr = (data.taxRoundingMode != null ? data.taxRoundingMode.ToString() : string.Empty);
            string orderStateStr = (data.orderState != null ? data.orderState.ToString() : string.Empty);
            string shipmentStateStr = (data.shipmentState != null ? data.shipmentState.ToString() : string.Empty);
            string paymentStateStr = (data.paymentState != null ? data.paymentState.ToString() : string.Empty);
            string inventoryModeStr = (data.inventoryMode != null ? data.inventoryMode.ToString() : string.Empty);

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.CompletedAt = data.completedAt;
            this.OrderNumber = data.orderNumber;
            this.CustomerId = data.customerId;
            this.CustomerEmail = data.customerEmail;
            this.AnonymousId = data.anonymousId;
            this.LineItems = Helper.GetListFromJsonArray<LineItem>(data.lineItems);
            this.CustomLineItems = Helper.GetListFromJsonArray<CustomLineItem>(data.customLineItems);
            this.TotalPrice = new Money(data.totalPrice);
            this.TaxedPrice = new TaxedPrice(data.taxedPrice);
            this.ShippingAddress = new Address(data.shippingAddress);
            this.BillingAddress = new Address(data.billingAddress);
            this.TaxMode = Enum.TryParse(taxModeStr, out taxMode) ? (TaxMode?)taxMode : null;
            this.TaxRoundingMode = Enum.TryParse(taxRoundingModeStr, out taxRoundingMode) ? (RoundingMode?)taxRoundingMode : null;
            this.CustomerGroup = new Reference(data.customerGroup);
            this.Country = data.country;
            this.OrderState = Enum.TryParse(orderStateStr, out orderState) ? (OrderState?)orderState : null;
            this.State = new Reference(data.state);
            this.ShipmentState = Enum.TryParse(shipmentStateStr, out shipmentState) ? (ShipmentState?)shipmentState : null;
            this.PaymentState = Enum.TryParse(paymentStateStr, out paymentState) ? (PaymentState?)paymentState : null;
            this.ShippingInfo = new ShippingInfo(data.shippingInfo);
            this.SyncInfo = Helper.GetListFromJsonArray<SyncInfo>(data.syncInfo);
            this.ReturnInfo = Helper.GetListFromJsonArray<ReturnInfo>(data.returnInfo);
            this.DiscountCodes = Helper.GetListFromJsonArray<DiscountCodeInfo>(data.discountCodes);
            this.LastMessageSequenceNumber = data.lastMessageSequenceNumber;
            this.Cart = new Reference(data.cart);
            this.Custom = new CustomFields.CustomFields(data.custom);
            this.PaymentInfo = new PaymentInfo(data.paymentInfo);
            this.Locale = data.locale;
            this.InventoryMode = Enum.TryParse(inventoryModeStr, out inventoryMode) ? (InventoryMode?)inventoryMode : null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Order order = obj as Order;

            if (order == null)
            {
                return false;
            }

            return (order.Id.Equals(this.Id));
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
