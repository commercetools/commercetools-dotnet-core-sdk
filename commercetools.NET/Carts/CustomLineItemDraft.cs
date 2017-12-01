using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// API representation for creating a new CustomLineItem.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#customlineitemdraft"/>
    public class CustomLineItemDraft
    {
        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public Money Money { get; set; }

        /// <summary>
        /// Slug
        /// </summary>
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }

        /// <summary>
        /// The given tax category will be used to select a tax rate when a cart has the TaxMode Platform.
        /// </summary>
        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; set; }

        /// <summary>
        /// An external tax rate can be set if the cart has the External TaxMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalTaxRate")]
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }

        /// <summary>
        /// The custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomLineItemDraft(LocalizedString name)
        {
            this.Name = name;
        }

        #endregion
    }
}
