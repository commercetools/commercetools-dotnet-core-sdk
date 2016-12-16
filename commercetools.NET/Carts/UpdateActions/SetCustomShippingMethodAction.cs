using commercetools.Common;
using commercetools.ShippingMethods;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets a custom shipping method (independent of the ShippingMethods defined in the project). 
    /// </summary>
    /// <remarks>
    /// The custom shipping method can be unset with the setShippingMethod action without the shippingMethod. Prerequisite: The cart must contain a shipping address.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-custom-shippingmethod"/>
    public class SetCustomShippingMethodAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Shipping method name
        /// </summary>
        [JsonProperty(PropertyName = "shippingMethodName")]
        public string ShippingMethodName { get; set; }

        /// <summary>
        /// The shipping rate used to determine the price.
        /// </summary>
        [JsonProperty(PropertyName = "shippingRate")]
        public ShippingRate ShippingRate { get; set; }

        /// <summary>
        /// The given tax category will be used to select a tax rate when a cart has the TaxMode Platform.
        /// </summary>
        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; set; }

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
        /// <param name="shippingMethodName">Shipping method name</param>
        public SetCustomShippingMethodAction(string shippingMethodName)
        {
            this.Action = "setCustomShippingMethod";
            this.ShippingMethodName = shippingMethodName;
        }

        #endregion
    }
}
