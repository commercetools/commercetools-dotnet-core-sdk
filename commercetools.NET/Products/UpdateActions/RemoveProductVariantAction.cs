using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// RemoveProductVariantAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#remove-productvariant"/>
    public class RemoveProductVariantAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// SKU
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
        /// Either id or sku must be specified.
        /// </remarks>
        /// <param name="id">Product variant ID</param>
        /// <param name="sku">Product variant SKU</param>
        public RemoveProductVariantAction(int? id = null, string sku = null)
        {
            if (!id.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either id or sku are required");
            }

            this.Action = "removeVariant";
            this.Id = id;
            this.Sku = sku;
        }

        #endregion
    }
}
