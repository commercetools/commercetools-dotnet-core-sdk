using commercetools.Common;
using commercetools.Products;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the addExternalImage update action and the upload of an image.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-messages.html#productimageadded-message"/>
    public class ProductImageAddedMessage : Message
    {
        #region Properties

        /// <summary>
        /// The variantId to identify the variant for which the image was added.
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int? VariantId { get; private set; }

        /// <summary>
        /// The image that was added.
        /// </summary>
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; private set; }

        /// <summary>
        /// True if it was applied only to staged.
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool? Staged { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductImageAddedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.VariantId = data.variantId;
            this.Image = new Image(data.image);
            this.Staged = data.staged;
        }

        #endregion
    }
}
