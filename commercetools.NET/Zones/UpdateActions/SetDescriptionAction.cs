using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Zones.UpdateActions
{
    /// <summary>
    /// SetDescriptionAction
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-zones.html#set-description"/>
    public class SetDescriptionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Deescription
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Deescription { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetDescriptionAction()
        {
            this.Action = "setDescription";
        }

        #endregion
    }
}
