using System;
using System.Collections.Generic;

using commercetools.Common;
using commercetools.Products;
using commercetools.Reviews;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.ProductProjections
{
    /// <summary>
    /// Product projection is a projected representation of a product that shows the product with its current or staged data.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productProjections.html#productprojection"/>
    public class ProductProjection
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "productType")]
        public Reference ProductType { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; private set; }

        [JsonProperty(PropertyName = "categories")]
        public List<Reference> Categories { get; private set; }

        [JsonProperty(PropertyName = "categoryOrderHints")]
        public JObject CategoryOrderHints { get; private set; }

        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; private set; }

        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; private set; }

        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; private set; }

        [JsonProperty(PropertyName = "searchKeywords")]
        public SearchKeywords SearchKeywords { get; private set; }

        [JsonProperty(PropertyName = "hasStagedChanges")]
        public bool? HasStagedChanges { get; private set; }

        [JsonProperty(PropertyName = "published")]
        public bool? Published { get; private set; }

        [JsonProperty(PropertyName = "masterVariant")]
        public ProductVariant MasterVariant { get; private set; }

        [JsonProperty(PropertyName = "variants")]
        public List<ProductVariant> Variants { get; private set; }

        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public Reference State { get; private set; }

        [JsonProperty(PropertyName = "reviewRatingStatistics")]
        public ReviewRatingStatistics ReviewRatingStatistics { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductProjection(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Key = data.key;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.ProductType = new Reference(data.productType);
            this.Name = new LocalizedString(data.name);
            this.Description = new LocalizedString(data.description);
            this.Slug = new LocalizedString(data.slug);
            this.Categories = Helper.GetListFromJsonArray<Reference>(data.categories);
            this.CategoryOrderHints = data.categoryOrderHints;
            this.MetaTitle = new LocalizedString(data.metaTitle);
            this.MetaDescription = new LocalizedString(data.metaDescription);
            this.MetaKeywords = new LocalizedString(data.metaKeywords);
            this.SearchKeywords = new SearchKeywords(data.searchKeywords);
            this.HasStagedChanges = data.hasStagedChanges;
            this.Published = data.published;
            this.MasterVariant = new ProductVariant(data.masterVariant);
            this.Variants = Helper.GetListFromJsonArray<ProductVariant>(data.variants);
            this.TaxCategory = new Reference(data.taxCategory);
            this.State = new Reference(data.state);
            this.ReviewRatingStatistics = new ReviewRatingStatistics(data.reviewRatingStatistics);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ProductProjection productProjection = obj as ProductProjection;

            if (productProjection == null)
            {
                return false;
            }

            return productProjection.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
