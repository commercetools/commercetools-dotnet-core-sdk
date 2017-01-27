using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom field for an existing product price.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-price-customfield"/>
    public class SetProductPriceCustomFieldAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Price ID
        /// </summary>
        [JsonProperty(PropertyName = "priceId")]
        public string PriceId { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        /// <remarks>
        /// If absent or null, this field is removed if it exists.
        /// </remarks>
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="priceId">Price ID</param>
        /// <param name="name">Field name</param>
        public SetProductPriceCustomFieldAction(string priceId, string name)
        {
            this.Action = "setProductPriceCustomField";
            this.PriceId = priceId;
            this.Name = name;
        }

        #endregion
    }
}
