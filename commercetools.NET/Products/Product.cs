using System;

using commercetools.Common;
using commercetools.Reviews;

using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// The full representation of a product combines the current and staged representations in a single representation.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#product"/>
    public class Product
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

        [JsonProperty(PropertyName = "masterData")]
        public ProductCatalogData MasterData { get; private set; }

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
        public Product(dynamic data)
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
            this.MasterData = new ProductCatalogData(data.masterData);
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
            Product product = obj as Product;

            if (product == null)
            {
                return false;
            }

            return product.Id.Equals(this.Id);
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
