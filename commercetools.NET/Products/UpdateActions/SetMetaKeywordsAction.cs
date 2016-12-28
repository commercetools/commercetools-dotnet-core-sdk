using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Set Meta Keywords
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-meta-keywords"/>
    public class SetMetaKeywordsAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// MetaKeywords
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
