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

        /// <summary>
        /// The unique ID of this CustomLineItem.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The name of this CustomLineItem.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        /// <summary>
        /// The cost to add to the cart. The amount can be negative.
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public Money Money { get; private set; }

        /// <summary>
        /// Set once the taxRate is set.
        /// </summary>
        [JsonProperty(PropertyName = "taxedPrice")]
        public TaxedItemPrice TaxedPrice { get; private set; }

        /// <summary>
        /// The total price of this custom line item.
        /// </summary>
        /// <remarks>
        /// If custom line item is discounted, then the totalPrice would be the discounted custom line item price multiplied by quantity. Otherwise a total price is just a money multiplied by the quantity. totalPrice may or may not include the taxes: it depends on the taxRate.includedInPrice property.
        /// </remarks>
        [JsonProperty(PropertyName = "totalPrice")]
        public Money TotalPrice { get; private set; }

        /// <summary>
        /// A unique String in the cart to identify this CustomLineItem.
        /// </summary>
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; private set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; private set; }

        /// <summary>
        /// Item states
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public List<ItemState> State { get; private set; }

        /// <summary>
        /// Reference to a TaxCategory
        /// </summary>
        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; private set; }

        /// <summary>
        /// Will be set automatically in the Platform TaxMode once the shipping address is set is set. 
        /// </summary>
        /// <remarks>
        /// For the External tax mode the tax rate has to be set explicitly with the ExternalTaxRateDraft.
        /// </remarks>
        [JsonProperty(PropertyName = "taxRate")]
        public TaxRate TaxRate { get; private set; }

        /// <summary>
        /// List of of DiscountedLineItemPriceForQuantity
        /// </summary>
        [JsonProperty(PropertyName = "discountedPricePerQuantity")]
        public List<DiscountedLineItemPriceForQuantity> DiscountedPricePerQuantity { get; private set; }

        /// <summary>
        /// Custom fields
        /// </summary>
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
            this.TaxedPrice = new TaxedItemPrice(data.taxedPrice);
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
