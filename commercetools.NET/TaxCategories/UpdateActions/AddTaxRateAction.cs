using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.TaxCategories.UpdateActions
{
    /// <summary>
    /// Add TaxRate
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#add-taxrate"/>
    public class AddTaxRateAction : UpdateAction
    {
        #region Properties

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
        /// <param name="taxRate">TaxRateDraft</param>
        public AddTaxRateAction(TaxRateDraft taxRate)
        {
            this.Action = "addTaxRate";
            this.TaxRate = taxRate;
        }

        #endregion
    }
}
