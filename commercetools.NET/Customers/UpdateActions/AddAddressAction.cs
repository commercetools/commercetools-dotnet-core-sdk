using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Adds an address to the customer's addresses array. Sets the address ID to be unique in the addresses list.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#add-address"/>
    public class AddAddressAction : UpdateAction
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
        /// <param name="address">Address</param>
        public AddAddressAction(Address address)
        {
            this.Action = "addAddress";
            this.Address = address;
        }

        #endregion
    }
}
