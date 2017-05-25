using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Adds a product variant in the given quantity to the cart.
    /// </summary>
    /// <remarks>
    /// If the cart already contains the product variant for the given supply and distribution channel, then only quantity of the LineItem is increased.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-lineitem"/>
    public class AddLineItemAction : UpdateAction
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
        /// Number
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// By providing supply channel information, you can unique identify inventory entries that should be reserved. Provided channel should have the role InventorySupply.
        /// </summary>
        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; set; }

        /// <summary>
        /// The channel is used to select a ProductPrice. Provided channel should have the role ProductDistribution.
        /// </summary>
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
        /// <param name="variantId">Id of an existing ProductVariant in the product</param>
        public AddLineItemAction(string productId, int variantId)
        {
            this.Action = "addLineItem";
            this.ProductId = productId;
            this.VariantId = variantId;
        }

        #endregion
    }
}
