using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Adds an existing address from the Customer’s addresses - referred to by its id - to the Customer’s billingAddressIds.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#add-billing-address-id"/>
    public class AddBillingAddressIdAction : UpdateAction
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
        public AddBillingAddressIdAction(string addressId)
        {
            this.Action = "addBillingAddressId";
            this.AddressId = addressId;
        }

        #endregion
    }
}
