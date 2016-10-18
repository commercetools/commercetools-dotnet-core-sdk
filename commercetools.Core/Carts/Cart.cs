using System;
using System.Collections.Generic;

using commercetools.Common;
using commercetools.Orders;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Carts
{
    /// <summary>
    /// A shopping cart holds product variants and can be ordered. Each cart either belongs to a registered customer or is an anonymous cart.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#cart"/>
    public class Cart
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; set; }

        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty(PropertyName = "lineItems")]
        public List<LineItem> LineItems { get; set; }

        [JsonProperty(PropertyName = "customLineItems")]
        public List<CustomLineItem> CustomLineItems { get; set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; set; }

        [JsonProperty(PropertyName = "taxedPrice")]
        public TaxedPrice TaxedPrice { get; set; }

        [JsonProperty(PropertyName = "cartState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartState? CartState { get; set; }

        [JsonProperty(PropertyName = "shippingAddress")]
        public Address ShippingAddress { get; set; }

        [JsonProperty(PropertyName = "billingAddress")]
        public Address BillingAddress { get; set; }

        [JsonProperty(PropertyName = "inventoryMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InventoryMode? InventoryMode { get; set; }

        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "shippingInfo")]
        public ShippingInfo ShippingInfo { get; set; }

        [JsonProperty(PropertyName = "discountCodes")]
        public List<DiscountCodeInfo> DiscountCodes { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; set; }

        [JsonProperty(PropertyName = "paymentInfo")]
        public PaymentInfo PaymentInfo { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Cart(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            CartState cartState;
            InventoryMode inventoryMode;

            string cartStateStr = (data.cartState != null ? data.cartState.ToString() : string.Empty);
            string inventoryModeStr = (data.inventoryMode != null ? data.inventoryMode.ToString() : string.Empty);

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.CustomerId = data.customerId;
            this.CustomerEmail = data.customerEmail;
            this.LineItems = Helper.GetListFromJsonArray<LineItem>(data.lineItems);
            this.CustomLineItems = Helper.GetListFromJsonArray<CustomLineItem>(data.customLineItems);
            this.TotalPrice = new Money(data.totalPrice);
            this.TaxedPrice = new TaxedPrice(data.taxedPrice);
            this.CartState = Enum.TryParse(cartStateStr, out cartState) ? (CartState?)cartState : null;
            this.ShippingAddress = new Address(data.shippingAddress);
            this.BillingAddress = new Address(data.billingAddress);
            this.InventoryMode = Enum.TryParse(inventoryModeStr, out inventoryMode) ? (InventoryMode?)inventoryMode : null;
            this.CustomerGroup = new Reference(data.customerGroup);
            this.Country = data.country;
            this.ShippingInfo = new ShippingInfo(data.shippingInfo);
            this.DiscountCodes = Helper.GetListFromJsonArray<DiscountCodeInfo>(data.discountCodes);
            this.Custom = new CustomFields.CustomFields(data.custom);
            this.PaymentInfo = new PaymentInfo(data.paymentInfo);
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
            Cart cart = obj as Cart;

            if (cart == null)
            {
                return false;
            }

            return cart.Id.Equals(this.Id);
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