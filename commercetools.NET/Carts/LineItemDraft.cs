using System.Collections.Generic;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// API representation for creating a new LineItem.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#lineitemdraft"/>
    public class LineItemDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "productId")]
        public string ProductId { get; set; }

        [JsonProperty(PropertyName = "variantId")]
        public int VariantId { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; set; }

        [JsonProperty(PropertyName = "distributionChannel")]
        public Reference DistributionChannel { get; set; }

        [JsonProperty(PropertyName = "customFieldsDraft")]
        public List<CustomFieldsDraft> Custom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LineItemDraft(string productId, int variantId, int quantity = 1)
        {
            this.ProductId = productId;
            this.VariantId = variantId;
            this.Quantity = quantity;
        }

        #endregion
    }
}
