using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// SetCustomerGroupAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-customergroup"/>
    public class SetCustomerGroupAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// If not defined, the customer group is unset.
        /// </summary>
        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetCustomerGroupAction()
        {
            this.Action = "setCustomerGroup";
        }

        #endregion
    }
}
