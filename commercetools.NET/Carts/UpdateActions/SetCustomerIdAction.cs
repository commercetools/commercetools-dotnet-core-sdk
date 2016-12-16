using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the customer ID of the cart.
    /// </summary>
    /// <remarks>
    /// When the customer ID is set, the LineItem prices are updated.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-customer-id"/>
    public class SetCustomerIdAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// If set, a customer with the given ID must exist in the project.
        /// </summary>
        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomerIdAction()
        {
            this.Action = "setCustomerId";
        }

        #endregion
    }
}
