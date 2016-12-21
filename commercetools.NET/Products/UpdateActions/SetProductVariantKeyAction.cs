using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Adds, changes or removes a key on a product variant.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-productvariant-key"/>
    public class SetProductVariantKeyAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Variant ID
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int? VariantId { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        /// <remarks>
        /// If left blank or set to null, the key is unset/removed.
        /// </remarks>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        /// <remarks>
        /// Defaults to true
        /// </remarks>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public SetProductVariantKeyAction(int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "setProductVariantKey";
        }

        #endregion
    }
}
