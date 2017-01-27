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

        /// <summary>
        /// The sequential ID of the variant within the product.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        /// <summary>
        /// The SKU of the variant.
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; private set; }

        /// <summary>
        /// User-specific unique identifier for the variant.
        /// </summary>
        /// <remarks>
        /// Product variant keys are different from product keys.
        /// </remarks>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        /// <summary>
        /// The prices of the variant. 
        /// </summary>
        /// <remarks>
        /// The prices does not contain two prices for the same price scope (same currency, country, customer group and channel).
        /// </remarks>
        [JsonProperty(PropertyName = "prices")]
        public List<Price> Prices { get; private set; }

        /// <summary>
        /// List of Attributes
        /// </summary>
        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute> Attributes { get; private set; }

        /// <summary>
        /// Only appears when price selection is used. 
        /// </summary>
        /// <remarks>
        /// This field cannot be used in a query predicate.
        /// </remarks>
        [JsonProperty(PropertyName = "price")]
        public Price Price { get; private set; }

        /// <summary>
        /// List of Images
        /// </summary>
        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; private set; }

        /// <summary>
        /// List of Assets
        /// </summary>
        [JsonProperty(PropertyName = "assets")]
        public List<Asset> Assets { get; private set; }

        /// <summary>
        /// The availability is set if the variant is tracked by the inventory.
        /// </summary>
        /// <remarks>
        /// The field might not contain the latest inventory state (it is eventually consistent) and can be used as an optimization to reduce calls to the inventory service.
        /// </remarks>
        [JsonProperty(PropertyName = "availability")]
        public ProductVariantAvailability Availability { get; private set; }

        /// <summary>
        /// Only appears in response to a Product Projection Search request to mark this variant as one that matches the search query.
        /// </summary>
        [JsonProperty(PropertyName = "isMatchingVariant")]
        public bool? IsMatchingVariant { get; private set; }

        /// <summary>
        /// Only appears when price selection is used.
        /// </summary>
        [JsonProperty(PropertyName = "scopedPrice")]
        public ScopedPrice ScopedPrice { get; private set; }

        /// <summary>
        /// Only appears in response to a Product Projection Search request when price selection is used.
        /// </summary>
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
            this.Assets = Helper.GetListFromJsonArray<Asset>(data.assets);
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
