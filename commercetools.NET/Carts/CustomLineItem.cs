using System;
using System.Collections.Generic;

using commercetools.Common;
using commercetools.CustomFields;
using commercetools.Orders;
using commercetools.Products;
using commercetools.TaxCategories;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// A custom line item is a generic item that can be added to the cart but is not bound to a product. You can use it for discounts (negative money), vouchers, complex cart rules, additional services or fees. You control the lifecycle of this item.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#customlineitem"/>
    public class CustomLineItem
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "money")]
        public Money Money { get; private set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; private set; }

        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; private set; }

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public List<ItemState> State { get; private set; }

        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; private set; }

        [JsonProperty(PropertyName = "taxRate")]
        public TaxRate TaxRate { get; private set; }

        [JsonProperty(PropertyName = "discountedPricePerQuantity")]
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public CustomLineItem(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Name = new LocalizedString(data.name);
            this.Money = new Money(data.money);
            this.TotalPrice = new Money(data.totalPrice);
            this.Slug = data.slug;
            this.Quantity = data.quantity;
            this.State = Helper.GetListFromJsonArray<ItemState>(data.state);
            this.TaxCategory = new Reference(data.taxCategory);
            this.TaxRate = new TaxRate(data.taxRate);
            this.DiscountedPricePerQuantity = Helper.GetListFromJsonArray<DiscountedLineItemPriceForQuantity>(data.discountedPricePerQuantity);
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
            CustomLineItem customLineItem = obj as CustomLineItem;

            if (customLineItem == null)
            {
                return false;
            }

            return customLineItem.Id.Equals(this.Id);
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
