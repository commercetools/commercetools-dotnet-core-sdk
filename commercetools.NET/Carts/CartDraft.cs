using System;
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
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#cart-draft"/>
    public class CartDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty(PropertyName = "anonymousId")]
        public string AnonymousId { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "inventoryMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InventoryMode? InventoryMode { get; set; }

        [JsonProperty(PropertyName = "taxMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaxMode? TaxMode { get; set; }

        [JsonProperty(PropertyName = "lineItems")]
        public List<LineItemDraft> LineItems { get; set; }

        [JsonProperty(PropertyName = "customLineItems")]
        public List<CustomLineItemDraft> CustomLineItems { get; set; }

        [JsonProperty(PropertyName = "shippingAddress")]
        public Address ShippingAddress { get; set; }

        [JsonProperty(PropertyName = "billingAddress")]
        public Address BillingAddress { get; set; }

        [JsonProperty(PropertyName = "shippingMethod")]
        public Reference ShippingMethod { get; set; }

        [JsonProperty(PropertyName = "externalTaxRateForShippingMethod")]
        public ExternalTaxRateDraft ExternalTaxRateForShippingMethod { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

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
