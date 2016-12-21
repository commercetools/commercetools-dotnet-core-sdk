using System;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom field for an existing Asset.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-asset-custom-field"/>
    public class SetAssetCustomFieldAction : UpdateAction
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
        /// Staged
        /// </summary>
        /// <remarks>
        /// Defaults to true
        /// </remarks>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        /// <summary>
        /// Asset ID
        /// </summary>
        [JsonProperty(PropertyName = "assetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// ResourceIdentifier to a Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

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
        /// <param name="assetId">Asset ID</param>
        /// <param name="name">Name</param>
        /// <param name="variantId">Variant ID</param>
        /// <param name="sku">Sku</param>
        public SetAssetCustomFieldAction(string assetId, string name, int? variantId = null, string sku = null)
        {
            if (!variantId.HasValue && string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Either variantId or sku are required");
            }

            this.Action = "setAssetCustomField";
            this.AssetId = assetId;
            this.Name = name;
            this.VariantId = variantId;
            this.Sku = sku;
        }

        #endregion
    }
}
