using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Sets the default billing address from the Customer’s addresses.
    /// </summary>
    /// <remarks>
    /// If the address is not in the Customer’s billing addresses it will be added to the Customer’s billingAddressIds.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-default-billing-address"/>
    public class SetDefaultBillingAddressAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// If not defined, the customer’s defaultBillingAddress is unset.
        /// </summary>
        [JsonProperty(PropertyName = "addressId")]
        public string AddressId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetDefaultBillingAddressAction()
        {
            this.Action = "setDefaultBillingAddress";
        }

        #endregion
    }
}
