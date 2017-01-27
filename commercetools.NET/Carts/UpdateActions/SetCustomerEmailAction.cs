using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// SetCustomerEmailAction.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-customer-email"/>
    public class SetCustomerEmailAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Email
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
