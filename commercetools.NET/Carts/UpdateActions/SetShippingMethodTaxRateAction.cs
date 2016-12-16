using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// A shipping method tax rate can be set if the cart has the External TaxMode.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-shippingmethod-taxrate"/>
    public class SetShippingMethodTaxRateAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ExternalTaxRate
        /// </summary>
        [JsonProperty(PropertyName = "externalTaxRate")]
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetShippingMethodTaxRateAction()
        {
            this.Action = "setShippingMethodTaxRate";
        }

        #endregion
    }
}
