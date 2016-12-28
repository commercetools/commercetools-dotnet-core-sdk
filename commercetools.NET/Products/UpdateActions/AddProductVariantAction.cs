using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Adds a product variant.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#add-productvariant"/>
    public class AddProductVariantAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Sku
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// Prices
        /// </summary>
        [JsonProperty(PropertyName = "prices")]
        public List<Price> Prices { get; set; }

        /// <summary>
        /// Images
        /// </summary>
        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; set; }

        /// <summary>
        /// Attributes
        /// </summary>
        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute> Attributes { get; set; }

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
        /// <param name="name">Name</param>
        public AddProductVariantAction(string name)
        {
            this.Action = "addVariant";
        }

        #endregion
    }
}
