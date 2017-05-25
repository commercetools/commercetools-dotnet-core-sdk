using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Moves an image to a new position within a product variant.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#move-image-to-position"/>
    public class MoveImageToPositionAction : UpdateAction
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
        /// Position
        /// </summary>
        [JsonProperty(PropertyName = "position")]
        public int Position { get; set; }

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
        /// <param name="position">Position</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public MoveImageToPositionAction(string imageUrl, int position, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "moveImageToPosition";
            this.ImageUrl = imageUrl;
            this.Position = position;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
