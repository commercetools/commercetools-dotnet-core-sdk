using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// Change name 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#change-name"/>
    public class ChangeSlugAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Slug
        /// </summary>
        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="slug">Slug</param>
        public ChangeSlugAction(LocalizedString slug)
        {
            this.Action = "changeSlug";
            this.Slug = slug;
        }

        #endregion
    }
}
