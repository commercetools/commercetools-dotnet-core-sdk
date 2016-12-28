using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the shipping address of the cart. 
    /// </summary>
    /// <remarks>
    /// Setting the shipping address also sets the TaxRate of the line items and calculates the TaxedPrice. If the address is not provided, the shipping address is unset, the taxedPrice is unset and the taxRates are unset in all line items.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-shipping-address"/>
    public class SetShippingAddressAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Address
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public Address Address  { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetShippingAddressAction()
        {
            this.Action = "setShippingAddress";
        }

        #endregion
    }
}
