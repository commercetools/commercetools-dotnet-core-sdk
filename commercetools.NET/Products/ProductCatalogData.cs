using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// ProductCatalogData
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#productcatalogdata"/>
    public class ProductCatalogData
    {
        #region Properties

        [JsonProperty(PropertyName = "published")]
        public bool? Published { get; private set; }

        [JsonProperty(PropertyName = "current")]
        public ProductData Current { get; private set; }

        [JsonProperty(PropertyName = "staged")]
        public ProductData Staged { get; private set; }

        [JsonProperty(PropertyName = "hasStagedChanges")]
        public bool? HasStagedChanges { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductCatalogData(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Published = data.published;
            this.Current = new ProductData(data.current);
            this.Staged = new ProductData(data.staged);
            this.HasStagedChanges = data.hasStagedChanges;
        }

        #endregion
    }
}
