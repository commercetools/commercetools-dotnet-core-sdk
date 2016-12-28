using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Sets the default shipping address from the Customer's addresses.
    /// </summary>
    /// <remarks>
    /// If the address is not in the Customer's shipping addresses it will be added to the Customer's shippingAddressIds.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-default-shipping-address"/>
    public class SetDefaultShippingAddressAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Address ID
        /// </summary>
        [JsonProperty(PropertyName = "addressId")]
        public string AddressId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetDefaultShippingAddressAction()
        {
            this.Action = "setDefaultShippingAddress";
        }

        #endregion
    }
}
