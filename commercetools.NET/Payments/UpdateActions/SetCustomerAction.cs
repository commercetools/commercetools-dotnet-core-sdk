using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets the reference to the customer. If not defined, the reference is unset.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#set-customer"/>
    public class SetCustomerAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// A reference to the customer this payment belongs to.
        /// </summary>
        [JsonProperty(PropertyName = "customer")]
        public Reference Customer { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomerAction()
        {
            this.Action = "setCustomer";
        }

        #endregion
    }
}
