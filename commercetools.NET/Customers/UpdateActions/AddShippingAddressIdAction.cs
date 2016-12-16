using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Adds an existing address from the Customer's addresses - referred to by its id - to the Customer's shippingAddressIds.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#add-shipping-address-id"/>
    public class AddShippingAddressIdAction : UpdateAction
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
        public AddShippingAddressIdAction(string addressId)
        {
            this.Action = "addShippingAddressId";
            this.AddressId = addressId;
        }

        #endregion
    }
}
