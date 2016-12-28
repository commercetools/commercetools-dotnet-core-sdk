using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the billing address of the cart. 
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-billing-address"/>
    public class SetBillingAddressAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Address
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetBillingAddressAction()
        {
            this.Action = "setBillingAddress";
        }

        #endregion
    }
}
