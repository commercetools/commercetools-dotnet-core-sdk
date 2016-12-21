using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// RemoveFromCategoryAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#remove-from-category"/>
    public class RemoveFromCategoryAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a Category 
        /// </summary>
        [JsonProperty(PropertyName = "category")]
        public Reference Category { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="category">Reference to a Category</param>
        public RemoveFromCategoryAction(Reference category)
        {
            this.Action = "removeFromCategory";
            this.Category = category;
        }

        #endregion
    }
}
