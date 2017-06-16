using System.Collections.Generic;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Carts
{
    /// <summary>
    /// API representation for creating a new Cart.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#cartdraft"/>
    public class CartDraft
    {
        #region Properties

        /// <summary>
        /// A three-digit currency code as per ISO 4217.
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Id of an existing Customer.
        /// </summary>
        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }

        /// <summary>
        /// Customer email
        /// </summary>
        [JsonProperty(PropertyName = "customerEmail")]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Assigns the new cart to an anonymous session (the customer has not signed up/in yet).
        /// </summary>
        [JsonProperty(PropertyName = "anonymousId")]
        public string AnonymousId { get; set; }

        /// <summary>
        /// A two-digit country code as per ISO 3166-1 alpha-2.
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        /// <summary>
        /// Default inventory mode is None.
        /// </summary>
        [JsonProperty(PropertyName = "inventoryMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InventoryMode? InventoryMode { get; set; }

        /// <summary>
        /// The default tax mode is Platform.
        /// </summary>
        [JsonProperty(PropertyName = "taxMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaxMode? TaxMode { get; set; }

        /// <summary>
        /// The default tax rounding mode is HalfEven.
        /// </summary>
        [JsonProperty(PropertyName = "taxRoundingMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoundingMode? TaxRoundingMode { get; set; }

        /// <summary>
        /// List of LineItemDrafts
        /// </summary>
        [JsonProperty(PropertyName = "lineItems")]
        public List<LineItemDraft> LineItems { get; set; }

        /// <summary>
        /// List of CustomLineItemDrafts
        /// </summary>
        [JsonProperty(PropertyName = "customLineItems")]
        public List<CustomLineItemDraft> CustomLineItems { get; set; }

        /// <summary>
        /// Shipping address
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddress")]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Billing address
        /// </summary>
        [JsonProperty(PropertyName = "billingAddress")]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Reference to a ShippingMethod
        /// </summary>
        [JsonProperty(PropertyName = "shippingMethod")]
        public Reference ShippingMethod { get; set; }

        /// <summary>
        /// An external tax rate can be set for the shippingMethod if the cart has the External TaxMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalTaxRateForShippingMethod")]
        public ExternalTaxRateDraft ExternalTaxRateForShippingMethod { get; set; }

        /// <summary>
        /// The custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        /// <summary>
        /// Must be one of the languages supported for this project.
        /// </summary>
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        /// <summary>
        /// The cart will be deleted automatically if it hasn’t been modified for the specified amount of days 
        /// and it is in the Active CartState. If a ChangeSubscription for carts exists, a ResourceDeleted notification will be sent.
        /// </summary>
        [JsonProperty(PropertyName = "deleteDaysAfterLastModification")]
        public int DeleteDaysAfterLastModification { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="currency">Currency</param>
        public CartDraft(string currency)
        {
            this.Currency = currency;
        }

        #endregion
    }
}
