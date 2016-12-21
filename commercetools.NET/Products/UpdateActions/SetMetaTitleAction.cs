using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Set Meta Title
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-meta-title"/>
    public class SetMetaTitleAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// MetaTitle
        /// </summary>
        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetMetaTitleAction()
        {
            this.Action = "setMetaTitle";
        }

        #endregion
    }
}
