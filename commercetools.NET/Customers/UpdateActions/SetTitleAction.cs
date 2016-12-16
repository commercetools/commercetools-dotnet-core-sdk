using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// SetTitleAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-title"/>
    public class SetTitleAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetTitleAction()
        {
            this.Action = "setTitle";
        }

        #endregion
    }
}
