using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.TaxCategories.UpdateActions
{
    /// <summary>
    /// Replace TaxRate
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#replace-taxrate"/>
    public class ReplaceTaxRateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// TaxRateId
        /// </summary>
        [JsonProperty(PropertyName = "taxRateId")]
        public string TaxRateId { get; set; }

        /// <summary>
        /// TaxRateDraft
        /// </summary>
        [JsonProperty(PropertyName = "taxRate")]
        public TaxRateDraft TaxRate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taxRateId">TaxRateId</param>
        /// <param name="taxRate">TaxRateDraft</param>
        public ReplaceTaxRateAction(string taxRateId, TaxRateDraft taxRate)
        {
            this.Action = "replaceTaxRate";
            this.TaxRateId = taxRateId;
            this.TaxRate = taxRate;
        }

        #endregion
    }
}
