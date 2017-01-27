using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Products
{
    /// <summary>
    /// The representation to be sent to the server when creating a new product.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#productdraft"/>
    public class ProductDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        [JsonProperty(PropertyName = "productType")]
        public ResourceIdentifier ProductType { get; set; }

        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public List<Reference> Categories { get; set; }

        [JsonProperty(PropertyName = "categoryOrderHints")]
        public JObject CategoryOrderHints { get; set; }

        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; set; }

        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; set; }

        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; set; }

        [JsonProperty(PropertyName = "masterVariant")]
        public ProductVariantDraft MasterVariant { get; set; }

        [JsonProperty(PropertyName = "variants")]
        public List<ProductVariantDraft> Variants { get; set; }

        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; set; }

        [JsonProperty(PropertyName = "searchKeywords")]
        public SearchKeywords SearchKeywords { get; set; }

        [JsonProperty(PropertyName = "state")]
        public Reference State { get; set; }

        [JsonProperty(PropertyName = "publish")]
        public bool Publish { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="productType">Product type</param>
        /// <param name="slug">Slug</param>
        public ProductDraft(LocalizedString name, ResourceIdentifier productType, LocalizedString slug)
        {
            this.Name = name;
            this.ProductType = productType;
            this.Slug = slug;
        }

        #endregion
    }
}
