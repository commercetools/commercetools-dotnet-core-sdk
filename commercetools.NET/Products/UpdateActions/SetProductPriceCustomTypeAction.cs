using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom type and fields for an existing product price.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-price-custom-type"/>
    public class SetProductPriceCustomTypeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ResourceIdentifier to a Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// Price ID
        /// </summary>
        [JsonProperty(PropertyName = "priceId")]
        public string PriceId { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        /// <remarks>
        /// Defaults to true
        /// </remarks>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        /// <summary>
        /// A valid JSON object, based on the FieldDefinitions of the Type 
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public JObject Fields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="priceId">Price ID</param>
        public SetProductPriceCustomTypeAction(string priceId)
        {
            this.Action = "setProductPriceCustomType";
            this.PriceId = priceId;
        }

        #endregion
    }
}
