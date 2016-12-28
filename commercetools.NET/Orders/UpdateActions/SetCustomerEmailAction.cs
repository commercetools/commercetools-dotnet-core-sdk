using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing value for customerEmail.
    /// </summary>
    /// <remarks>
    /// This action does not change the customer email on the Cart the order has been created from.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#set-customer-email"/>
    public class SetCustomerEmailAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Customer email
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomerEmailAction()
        {
            this.Action = "setCustomerEmail";
        }

        #endregion
    }
}
