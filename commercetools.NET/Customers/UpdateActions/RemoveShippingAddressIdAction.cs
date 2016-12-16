using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Removes an existing shipping address from the Customer's shippingAddressesIds.
    /// </summary>
    /// <remarks>
    /// If the shipping address is the Customer's default shipping address the Customer's defaultShippingAddressId will be unset.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#remove-shipping-address-id"/>
    public class RemoveShippingAddressIdAction : UpdateAction
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
        /// <param name="addressId">Address ID</param>
        public RemoveShippingAddressIdAction(string addressId)
        {
            this.Action = "removeShippingAddressId";
            this.AddressId = addressId;
        }

        #endregion
    }
}
