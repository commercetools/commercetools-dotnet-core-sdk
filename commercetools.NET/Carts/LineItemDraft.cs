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

        /// <summary>
        /// Id of an existing Product.
        /// </summary>
        [JsonProperty(PropertyName = "productId")]
        public string ProductId { get; set; }

        /// <summary>
        /// Id of an existing ProductVariant in the product.
        /// </summary>
        [JsonProperty(PropertyName = "variantId")]
        public int VariantId { get; set; }

        /// <summary>
        /// Quantity - Defaults to 1
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Reference to a Channel
        /// </summary>
        /// <remarks>
        /// By providing supply channel information, you can unique identify inventory entries that should be reserved. Provided channel should have the role InventorySupply.
        /// </remarks>
        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; set; }

        /// <summary>
        /// Reference to a Channel
        /// </summary>
        /// <remarks>
        /// The channel is used to select a ProductPrice. Provided channel should have the role ProductDistribution.
        /// </remarks>
        [JsonProperty(PropertyName = "distributionChannel")]
        public Reference DistributionChannel { get; set; }

        /// <summary>
        /// An external tax rate can be set if the cart has the External TaxMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalTaxRate")]
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }

        /// <summary>
        /// The custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="productId">Id of an existing Product.</param>
        /// <param name="variantId">Id of an existing ProductVariant in the product.</param>
        public LineItemDraft(string productId, int variantId)
        {
            this.ProductId = productId;
            this.VariantId = variantId;
        }

        #endregion
    }
}
