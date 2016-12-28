using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets the locale. Must be one of the languages supported for this Project.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-locale"/>
    public class SetLocaleAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// String conforming to IETF language tag.
        /// </summary>
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetLocaleAction()
        {
            this.Action = "setLocale";
        }

        #endregion
    }
}
