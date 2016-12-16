using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Sets a string that uniquely identifies a customer. It can be used to create more human-readable (in contrast to ID) identifier for the customer.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-customer-number"/>
    public class SetCustomerNumberAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Customer Number
        /// </summary>
        [JsonProperty(PropertyName = "customerNumber")]
        public string CustomerNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomerNumberAction()
        {
            this.Action = "setCustomerNumber";
        }

        #endregion
    }
}
