using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Replaces the address with the given ID, with the new address in the customer's addresses array. The new address will have the same ID.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#change-address"/>
    public class ChangeAddressAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Address ID
        /// </summary>
        [JsonProperty(PropertyName = "addressId")]
        public string AddressId { get; set; }

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
        /// <param name="addressId">Address ID</param>
        /// <param name="address">Address</param>
        public ChangeAddressAction(string addressId, Address address)
        {
            this.Action = "changeAddress";
            this.AddressId = addressId;
            this.Address = address;
        }

        #endregion
    }
}
