using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.TaxCategories.UpdateActions
{
    /// <summary>
    /// Remove TaxRate
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#remove-taxrate"/>
    public class RemoveTaxRateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// TaxRateId
        /// </summary>
        [JsonProperty(PropertyName = "taxRateId")]
        public string TaxRateId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taxRateId">TaxRateId</param>
        public RemoveTaxRateAction(string taxRateId)
        {
            this.Action = "removeTaxRate";
            this.TaxRateId = taxRateId;
        }

        #endregion
    }
}
