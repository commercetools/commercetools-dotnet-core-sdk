using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Change IsSearchable
    /// </summary>
    /// <remarks>
    /// Following this update the products are reindexed asynchronously to reflect this change on the search endpoint. When enabling search on an existing attribute type definition, the constraint regarding the maximum size of a searchable attribute will not be enforced. Instead, product attribute definitions exceeding this limit will be treated as not searchable and will not be available for full-text search.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#change-attributedefinition-issearchable"/>
    public class ChangeIsSearchableAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// The name of the attribute definition to update.
        /// </summary>
        [JsonProperty(PropertyName = "attributeName")]
        public string AttributeName { get; set; }

        /// <summary>
        /// Is Searchable
        /// </summary>
        [JsonProperty(PropertyName = "isSearchable")]
        public bool IsSearchable { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The name of the attribute definition to update.</param>
        /// <param name="isSearchable">Is Searchable</param>
        public ChangeIsSearchableAction(string attributeName, bool isSearchable)
        {
            this.Action = "changeIsSearchable";
            this.AttributeName = attributeName;
            this.IsSearchable = isSearchable;
        }

        #endregion
    }
}
