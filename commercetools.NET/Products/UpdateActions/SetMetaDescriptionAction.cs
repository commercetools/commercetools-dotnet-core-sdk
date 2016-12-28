using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Set Meta Description
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-meta-description"/>
    public class SetMetaDescriptionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// MetaDescription
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
