using System;
using System.Collections.Generic;

using commercetools.Common;
using commercetools.Orders;
using commercetools.Products;
using commercetools.TaxCategories;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Carts
{
    /// <summary>
    /// A line item is a snapshot of a product variant at the time it was added to the cart.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#lineitem"/>
    public class LineItem
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "productId")]
        public string ProductId { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "productSlug")]
        public LocalizedString ProductSlug { get; private set; }

        [JsonProperty(PropertyName = "variant")]
        public ProductVariant Variant { get; private set; }

        [JsonProperty(PropertyName = "price")]
        public Price Price { get; private set; }

        [JsonProperty(PropertyName = "taxedPrice")]
        public TaxedItemPrice TaxedPrice { get; private set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; private set; }

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public List<ItemState> State { get; private set; }

        [JsonProperty(PropertyName = "taxRate")]
        public TaxRate TaxRate { get; private set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; private set; }

        [JsonProperty(PropertyName = "distributionChannel")]
        public Reference DistributionChannel { get; private set; }

        [JsonProperty(PropertyName = "discountedPricePerQuantity")]
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; private set; }

        [JsonProperty(PropertyName = "priceMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LineItemPriceMode? PriceMode { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public LineItem(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            LineItemPriceMode priceMode;
            string priceModeStr = (data.priceMode != null ? data.priceMode.ToString() : string.Empty);

            this.Id = data.id;
            this.ProductId = data.productId;
            this.Name = new LocalizedString(data.name);
            this.ProductSlug = new LocalizedString(data.productSlug);
            this.Variant = new ProductVariant(data.variant);
            this.Price = new Price(data.price);
            this.TaxedPrice = new TaxedItemPrice(data.taxedPrice);
            this.TotalPrice = new Money(data.totalPrice);
            this.Quantity = data.quantity;
            this.State = Helper.GetListFromJsonArray<ItemState>(data.State);
            this.TaxRate = new TaxRate(data.taxRate);
            this.SupplyChannel = new Reference(data.supplyChannel);
            this.DistributionChannel = new Reference(data.distributionChannel);
            this.DiscountedPricePerQuantity = Helper.GetListFromJsonArray<DiscountedLineItemPriceForQuantity>(data.discountedPricePerQuantity);
            this.PriceMode = Enum.TryParse(priceModeStr, out priceMode) ? (LineItemPriceMode?)priceMode : null;
            this.Custom = new CustomFields.CustomFields(data.custom);
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
            LineItem lineItem = obj as LineItem;

            if (lineItem == null)
            {
                return false;
            }

            return lineItem.Id.Equals(this.Id);
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
