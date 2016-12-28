using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Updates the tax rates and the prices and optionally the line item product data.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#recalculate"/>
    public class RecalculateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// If set to true, the line item product data (name, variant and productType) will also be updated. If set to false, only the prices and tax rates will be updated.
        /// </summary>
        [JsonProperty(PropertyName = "updateProductData")]
        public bool UpdateProductData { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecalculateAction()
        {
            this.Action = "recalculate";
        }

        #endregion
    }
}
