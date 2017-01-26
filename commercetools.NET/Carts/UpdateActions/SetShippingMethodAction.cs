using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the ShippingMethod.
    /// </summary>
    /// <remarks>
    /// Prerequisite: The cart must contain a shipping address.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-shippingmethod"/>
    public class SetShippingMethodAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Shipping method
        /// </summary>
        [JsonProperty(PropertyName = "shippingMethod")]
        public Reference ShippingMethod { get; set; }

        /// <summary>
        /// An external tax rate can be set if the cart has the External TaxMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalTaxRate")]
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetShippingMethodAction()
        {
            this.Action = "setShippingMethod";
        }

        #endregion
    }
}
