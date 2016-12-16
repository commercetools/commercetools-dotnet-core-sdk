using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// The representation to be sent to the server when creating a new product variant.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#productvariantdraft"/>
    public class ProductVariantDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "prices")]
        public List<PriceDraft> Prices { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; set; }

        [JsonProperty(PropertyName = "assets")]
        public List<AssetDraft> Assets { get; set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute> Attributes { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProductVariantDraft()
        {
        }

        #endregion
    }
}
