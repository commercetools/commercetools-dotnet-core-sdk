using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing value for shippingAddress.
    /// </summary>
    /// <remarks>
    /// Changing the shipping address does not recalculate the cart. The taxes might not fit to the shipping address anymore. This action does not change the shipping address on the Cart the order has been created from.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-shipping-address"/>
    public class SetShippingAddressAction : UpdateAction
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
        public SetShippingAddressAction()
        {
            this.Action = "setShippingAddress";
        }

        #endregion
    }
}
