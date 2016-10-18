using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// ProductVariant
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#productvariant"/>
    public class ProductVariant
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "prices")]
        public List<Price> Prices { get; private set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute> Attributes { get; private set; }

        [JsonProperty(PropertyName = "price")]
        public Price Price { get; private set; }

        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; private set; }

        [JsonProperty(PropertyName = "availability")]
        public ProductVariantAvailability Availability { get; private set; }

        [JsonProperty(PropertyName = "isMatchingVariant")]
        public bool? IsMatchingVariant { get; private set; }

        [JsonProperty(PropertyName = "scopedPrice")]
        public ScopedPrice ScopedPrice { get; private set; }

        [JsonProperty(PropertyName = "scopedPriceDiscounted")]
        public bool? ScopedPriceDiscounted { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductVariant(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Sku = data.sku;
            this.Key = data.key;
            this.Prices = Helper.GetListFromJsonArray<Price>(data.prices);
            this.Attributes = Helper.GetListFromJsonArray<Attribute>(data.attributes);
            this.Price = new Price(data.price);
            this.Images = Helper.GetListFromJsonArray<Image>(data.images);
            this.Availability = new ProductVariantAvailability(data.availability);
            this.IsMatchingVariant = data.isMatchingVariant;
            this.ScopedPrice = new ScopedPrice(data.scopedPrice);
            this.ScopedPriceDiscounted = data.scopedPriceDiscounted;
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
            ProductVariant productVariant = obj as ProductVariant;

            if (productVariant == null)
            {
                return false;
            }

            return productVariant.Id.Equals(this.Id);
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