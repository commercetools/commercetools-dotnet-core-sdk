using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Changes the tax RoundingMode of a cart.
    /// </summary>
    /// <remarks>
    /// When changing the tax rounding mode, the taxes are recalculated.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#change-tax-roundingmode"/>
    public class ChangeTaxRoundingModeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// RoundingMode
        /// </summary>
        [JsonProperty(PropertyName = "taxRoundingMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoundingMode TaxRoundingMode { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taxRoundingMode">RoundingMode</param>
        public ChangeTaxRoundingModeAction(RoundingMode taxRoundingMode)
        {
            this.Action = "changeTaxRoundingMode";
            this.TaxRoundingMode = taxRoundingMode;
        }

        #endregion
    }
}
