using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing value for billingAddress.
    /// </summary>
    /// <remarks>
    /// This action does not change the billing address on the Cart the order has been created from.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-billing-address"/>
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
