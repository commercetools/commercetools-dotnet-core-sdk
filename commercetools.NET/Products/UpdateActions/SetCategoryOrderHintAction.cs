using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// SetCategoryOrderHintAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-category-order-hint"/>
    public class SetCategoryOrderHintAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Id of a Category the product belongs to
        /// </summary>
        [JsonProperty(PropertyName = "categoryId")]
        public string CategoryId { get; set; }

        /// <summary>
        /// String representing a number between 0 and 1
        /// </summary>
        [JsonProperty(PropertyName = "orderHint")]
        public string OrderHint { get; set; }

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
        /// <param name="categoryId">Id of a Category the product belongs to</param>
        public SetCategoryOrderHintAction(string categoryId)
        {
            this.Action = "setCategoryOrderHint";
            this.CategoryId = categoryId;
        }

        #endregion
    }
}
