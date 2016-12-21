using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Set SearchKeywords
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-searchkeywords"/>
    public class SetSearchKeywordsAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// SearchKeywords
        /// </summary>
        [JsonProperty(PropertyName = "searchKeywords")]
        public SearchKeywords SearchKeywords { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        /// <remarks>
        /// Defaults to true
        /// </remarks>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchKeywords">SearchKeywords</param>
        public SetSearchKeywordsAction(SearchKeywords searchKeywords)
        {
            this.Action = "setSearchKeywords";
            this.SearchKeywords = searchKeywords;
        }

        #endregion
    }
}
