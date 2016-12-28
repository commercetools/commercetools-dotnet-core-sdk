using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Sets the given variant as the new master variant. The previous master variant is added to the back of the list of variants.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#change-master-variant"/>
    public class ChangeMasterVariantAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Variant ID
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int? VariantId { get; set; }

        /// <summary>
        /// Variant SKU
        /// </summary>
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
        /// <remarks>
        /// Either variantId or sku must be specified.
        /// </remarks>
        /// <param name="variantId">Product variant ID</param>
        /// <param name="sku">Product variant SKU</param>
        public ChangeMasterVariantAction(int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "changeMasterVariant";
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
