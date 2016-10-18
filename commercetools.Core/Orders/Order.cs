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

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "completedAt")]
        public DateTime? CompletedAt { get; private set; }

        [JsonProperty(PropertyName = "orderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; private set; }

        [JsonProperty(PropertyName = "customerEmail")]
        public string CustomerEmail { get; private set; }

        [JsonProperty(PropertyName = "anonymousId")]
        public string AnonymousId { get; private set; }

        [JsonProperty(PropertyName = "lineItems")]
        public List<LineItem> LineItems { get; private set; }

        [JsonProperty(PropertyName = "customLineItems")]
        public List<CustomLineItem> CustomLineItems { get; private set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; private set; }

        [JsonProperty(PropertyName = "taxedPrice")]
        public TaxedPrice TaxedPrice { get; private set; }

        [JsonProperty(PropertyName = "shippingAddress")]
        public Address ShippingAddress { get; private set; }

        [JsonProperty(PropertyName = "billingAddress")]
        public Address BillingAddress { get; private set; }

        [JsonProperty(PropertyName = "taxMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaxMode? TaxMode { get; private set; }

        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; private set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; private set; }

        [JsonProperty(PropertyName = "orderState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderState? OrderState { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public Reference State { get; private set; }

        [JsonProperty(PropertyName = "shipmentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipmentState? ShipmentState { get; private set; }

        [JsonProperty(PropertyName = "paymentState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentState? PaymentState { get; private set; }

        [JsonProperty(PropertyName = "shippingInfo")]
        public ShippingInfo ShippingInfo { get; private set; }

        [JsonProperty(PropertyName = "syncInfo")]
        public List<SyncInfo> SyncInfo { get; private set; }

        [JsonProperty(PropertyName = "returnInfo")]
        public List<ReturnInfo> ReturnInfo { get; private set; }

        [JsonProperty(PropertyName = "discountCodes")]
        public List<DiscountCodeInfo> DiscountCodes { get; private set; }

        [JsonProperty(PropertyName = "lastMessageSequenceNumber")]
        public int? LastMessageSequenceNumber { get; private set; }

        [JsonProperty(PropertyName = "cart")]
        public Reference Cart { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        [JsonProperty(PropertyName = "paymentInfo")]
        public PaymentInfo PaymentInfo { get; private set; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; private set; }

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
            OrderState orderState;
            ShipmentState shipmentState;
            PaymentState paymentState;
            InventoryMode inventoryMode;

            string taxModeStr = (data.taxMode != null ? data.taxMode.ToString() : string.Empty);
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