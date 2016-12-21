using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Adds, changes or removes a SKU on a product variant.
    /// </summary>
    /// <remarks>
    /// A SKU can only be changed or removed from a variant through this operation if there is no inventory entry associated with that SKU.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-sku"/>
    public class SetSkuAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Variant ID
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int VariantId { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        /// <remarks>
        /// If left blank or set to null, the sku is unset/removed.
        /// </remarks>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="variantId">Variant ID</param>
        public SetSkuAction(int variantId)
        {
            this.Action = "setSku";
            this.VariantId = variantId;
        }

        #endregion
    }
}
