using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// SetMetaKeywordsAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#set-meta-keywords"/>
    public class SetMetaKeywordsAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Meta Keywords
        /// </summary>
        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetMetaKeywordsAction()
        {
            this.Action = "setMetaKeywords";
        }

        #endregion
    }
}
