using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Subscriptions.UpdateActions
{
    /// <summary>
    /// Set key
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#set-key"/>
    public class SetKeyAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Key
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetKeyAction()
        {
            this.Action = "setKey";
        }

        #endregion
    }
}
