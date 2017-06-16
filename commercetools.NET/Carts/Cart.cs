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

        /// <summary>
        /// The unique ID of the cart.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The current version of the cart.
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
        /// The sum of all totalPrice fields of the lineItems and customLineItems, as well as the price field of shippingInfo (if it exists).
        /// </summary>
        /// <remarks>
        /// totalPrice may or may not include the taxes: it depends on the taxRate.includedInPrice property of each price.
        /// </remarks>
        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; private set; }

        /// <summary>
        /// Not set until the shipping address is set. Will be set automatically in the Platform TaxMode.
        /// </summary>
        /// <remarks>
        /// For the External tax mode it will be set as soon as the external tax rates for all line items, custom line items, and shipping in the cart are set.
        /// </remarks>
        [JsonProperty(PropertyName = "taxedPrice")]
        public TaxedPrice TaxedPrice { get; private set; }

        /// <summary>
        /// Cart State
        /// </summary>
        [JsonProperty(PropertyName = "cartState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CartState? CartState { get; private set; }

        /// <summary>
        /// The shipping address is also used to determine tax rate of the line items.
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddress")]
        public Address ShippingAddress { get; private set; }

        /// <summary>
        /// Billing Address
        /// </summary>
        [JsonProperty(PropertyName = "billingAddress")]
        public Address BillingAddress { get; private set; }

        /// <summary>
        /// Inventory Mode
        /// </summary>
        [JsonProperty(PropertyName = "inventoryMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InventoryMode? InventoryMode { get; private set; }

        /// <summary>
        /// Tax Mode
        /// </summary>
        [JsonProperty(PropertyName = "taxMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaxMode? TaxMode { get; private set; }

        /// <summary>
        /// When calculating taxes for taxedPrice, the selected mode is used for rounding.
        /// </summary>
        [JsonProperty(PropertyName = "taxRoundingMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoundingMode? TaxRoundingMode { get; private set; }

        /// <summary>
        /// Reference to a CustomerGroup
        /// </summary>
        /// <remarks>
        /// Set automatically when the customer is set and the customer is a member of a customer group. Used for product variant price selection.
        /// </remarks>
        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; private set; }

        /// <summary>
        /// A two-digit country code as per ISO 3166-1 alpha-2. Used for product variant price selection.
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; private set; }

        /// <summary>
        /// Set automatically once the ShippingMethod is set.
        /// </summary>
        [JsonProperty(PropertyName = "shippingInfo")]
        public ShippingInfo ShippingInfo { get; private set; }

        /// <summary>
        /// List of DiscountCodeInfo
        /// </summary>
        [JsonProperty(PropertyName = "discountCodes")]
        public List<DiscountCodeInfo> DiscountCodes { get; private set; }

        /// <summary>
        /// The custom fields.
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
        /// The cart will be deleted automatically if it hasn’t been modified 
        /// for the specified amount of days and it is in the Active CartState.
        /// </summary>
        [JsonProperty(PropertyName = "deleteDaysAfterLastModification")]
        public int? DeleteDaysAfterLastModification { get; private set; }

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
            TaxMode taxMode;
            RoundingMode taxRoundingMode;

            string cartStateStr = (data.cartState != null ? data.cartState.ToString() : string.Empty);
            string inventoryModeStr = (data.inventoryMode != null ? data.inventoryMode.ToString() : string.Empty);
            string taxModeStr = (data.taxMode != null ? data.taxMode.ToString() : string.Empty);
            string taxRoundingModeStr = (data.taxRoundingMode != null ? data.taxRoundingMode.ToString() : string.Empty);

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.CustomerId = data.customerId;
            this.CustomerEmail = data.customerEmail;
            this.AnonymousId = data.anonymousId;
            this.LineItems = Helper.GetListFromJsonArray<LineItem>(data.lineItems);
            this.CustomLineItems = Helper.GetListFromJsonArray<CustomLineItem>(data.customLineItems);
            this.TotalPrice = new Money(data.totalPrice);
            this.TaxedPrice = new TaxedPrice(data.taxedPrice);
            this.CartState = Enum.TryParse(cartStateStr, out cartState) ? (CartState?)cartState : null;
            this.ShippingAddress = new Address(data.shippingAddress);
            this.BillingAddress = new Address(data.billingAddress);
            this.InventoryMode = Enum.TryParse(inventoryModeStr, out inventoryMode) ? (InventoryMode?)inventoryMode : null;
            this.TaxMode = Enum.TryParse(taxModeStr, out taxMode) ? (TaxMode?)taxMode : null;
            this.TaxRoundingMode = Enum.TryParse(taxRoundingModeStr, out taxRoundingMode) ? (RoundingMode?)taxRoundingMode : null;
            this.CustomerGroup = new Reference(data.customerGroup);
            this.Country = data.country;
            this.ShippingInfo = new ShippingInfo(data.shippingInfo);
            this.DiscountCodes = Helper.GetListFromJsonArray<DiscountCodeInfo>(data.discountCodes);
            this.Custom = new CustomFields.CustomFields(data.custom);
            this.PaymentInfo = new PaymentInfo(data.paymentInfo);
            this.Locale = data.locale;
            this.DeleteDaysAfterLastModification = data.deleteDaysAfterLastModification;
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