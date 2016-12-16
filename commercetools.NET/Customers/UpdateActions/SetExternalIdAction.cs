using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// Sets a new ID which can be used as additional identifier for external Systems like CRM or ERP.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-external-id"/>
    public class SetExternalIdAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// External ID
        /// </summary>
        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetExternalIdAction()
        {
            this.Action = "setExternalId";
        }

        #endregion
    }
}
