using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// SetLastNameAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-last-name"/>
    public class SetLastNameAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Last Name
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetLastNameAction()
        {
            this.Action = "setLastName";
        }

        #endregion
    }
}
