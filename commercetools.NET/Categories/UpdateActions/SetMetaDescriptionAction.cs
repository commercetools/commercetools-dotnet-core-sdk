using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// SetMetaDescriptionAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#set-meta-description"/>
    public class SetMetaDescriptionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Meta Description
        /// </summary>
        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetMetaDescriptionAction()
        {
            this.Action = "setMetaDescription";
        }

        #endregion
    }
}
