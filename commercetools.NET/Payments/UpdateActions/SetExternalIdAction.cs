using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets a user-defined ID which can be used as additional identifier for external systems, like the systems involved in order or receivables management. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#set-externalid"/>
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
