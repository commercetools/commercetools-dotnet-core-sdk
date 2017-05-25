using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Add to Category
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#add-to-category"/>
    public class AddToCategoryAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Reference to a Category 
        /// </summary>
        [JsonProperty(PropertyName = "category")]
        public Reference Category { get; set; }

        /// <summary>
        /// String representing a number between 0 and 1
        /// </summary>
        [JsonProperty(PropertyName = "orderHint")]
        public string OrderHint { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        /// <remarks>
        /// Defaults to true
        /// </remarks>
        [JsonProperty(PropertyName = "staged")]
        public bool? Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="category">Reference to a Category</param>
        public AddToCategoryAction(Reference category)
        {
            this.Action = "addToCategory";
            this.Category = category;
        }

        #endregion
    }
}
