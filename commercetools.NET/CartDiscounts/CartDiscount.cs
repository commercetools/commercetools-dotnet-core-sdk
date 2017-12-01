using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.CartDiscounts
{
    public class CartDiscount
    {
        #region Properties

        /// <summary>
        /// The unique ID of the cart discount.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The current version of the cart discount.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        /// <summary>
        /// Created At
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        /// Last Modified At
        /// </summary>
        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public CartDiscountValue Value { get; private set; }

        /// <summary>
        /// Cart Discount Predicate
        /// </summary>
        [JsonProperty(PropertyName = "cartPredicate")]
        public string CartPredicate { get; private set; }

        /// <summary>
        /// Cart Discount Target
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public CartDiscountTarget Target { get; private set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        [JsonProperty(PropertyName = "sortOrder")]
        public string SortOrder { get; private set; }

        /// <summary>
        /// Only active discount can be applied to the cart.
        /// </summary>
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Valid from
        /// </summary>
        [JsonProperty(PropertyName = "validFrom")]
        public DateTime? ValidFrom { get; private set; }

        /// <summary>
        /// Valid until
        /// </summary>
        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; private set; }

        /// <summary>
        /// States whether the discount can only be used in a connection with a DiscountCode.
        /// </summary>
        [JsonProperty(PropertyName = "requiresDiscountCode")]
        public bool RequiresDiscountCode { get; private set; }

        /// <summary>
        /// The platform will generate this array from the predicate. It contains the references of all the resources that are addressed in the predicate.
        /// </summary>
        [JsonProperty(PropertyName = "references")]
        public List<Reference> References { get; private set; }

        /// <summary>
        /// Specifies whether the application of this discount causes the following discounts to be ignored. Defaults to Stacking.
        /// </summary>
        [JsonProperty(PropertyName = "stackingMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StackingMode? StackingMode { get; private set; }

        #endregion

        public CartDiscount(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            StackingMode stackingMode;
            string stackingModeStr = (data.stackingMode != null ? data.stackingMode.ToString() : string.Empty);

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Name = new LocalizedString(data.name);
            this.Description = new LocalizedString(data.description);
            this.SortOrder = data.sortOrder;
            this.ValidFrom = data.validFrom;
            this.ValidUntil = data.validUntil;
            this.IsActive = data.isActive;
            this.RequiresDiscountCode = data.requiresDiscountCode;
            this.CartPredicate = data.cartPredicate;
            this.Value = CartDiscountValueFactory.Create(data.value);
            this.Target = new CartDiscountTarget(data.target);
            this.References = Helper.GetListFromJsonArray<Reference>(data.references);
            this.StackingMode = Enum.TryParse(stackingModeStr, out stackingMode) ? (StackingMode?)stackingMode : null;
        }
    }
}
