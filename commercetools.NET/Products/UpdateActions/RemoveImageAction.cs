using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Removes a product image and deletes it from the Content Delivery Network (it would not be deleted from the CDN in case of external image).
    /// </summary>
    /// <remarks>
    /// Deletion from the CDN is not instant, which means the image file itself will stay available for some time after the deletion.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#remove-image"/>
    public class RemoveImageAction : UpdateAction
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
        /// The URL of the image
        /// </summary>
        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get; set; }

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
        /// <param name="imageUrl">The URL of the image</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public RemoveImageAction(string imageUrl, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "removeImage";
            this.ImageUrl = imageUrl;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
