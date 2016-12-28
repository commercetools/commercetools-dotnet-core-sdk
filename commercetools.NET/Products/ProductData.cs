using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Products
{
    /// <summary>
    /// ProductData
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#productdata"/>
    public class ProductData
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "categories")]
        public List<Reference> Categories { get; private set; }

        [JsonProperty(PropertyName = "categoryOrderHints")]
        public JObject CategoryOrderHints { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; private set; }

        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; private set; }

        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; private set; }

        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; private set; }

        [JsonProperty(PropertyName = "masterVariant")]
        public ProductVariant MasterVariant { get; private set; }

        [JsonProperty(PropertyName = "variants")]
        public List<ProductVariant> Variants { get; private set; }

        [JsonProperty(PropertyName = "searchKeywords")]
        public SearchKeywords SearchKeywords { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductData(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Name = new LocalizedString(data.name);
            this.Categories = Helper.GetListFromJsonArray<Reference>(data.categories);
            this.CategoryOrderHints = new JObject(data.categoryOrderHints);
            this.Description = new LocalizedString(data.description);
            this.Slug = new LocalizedString(data.slug);
            this.MetaTitle = new LocalizedString(data.metaTitle);
            this.MetaDescription = new LocalizedString(data.metaDescription);
            this.MetaKeywords = new LocalizedString(data.metaKeywords);
            this.MasterVariant = new ProductVariant(data.masterVariant);
            this.Variants = Helper.GetListFromJsonArray<ProductVariant>(data.variants);
            this.SearchKeywords = new SearchKeywords(data.searchKeywords);
        }

        #endregion
    }
}
