using System;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts
{
    public class CartDiscountDraft
    {
        #region properties

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

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; private set; }

        [JsonProperty(PropertyName = "validFrom")]
        public DateTime? ValidFrom { get; private set; }

        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; private set; }

        /// <summary>
        /// States whether the discount can only be used in a connection with a DiscountCode.
        /// </summary>
        [JsonProperty(PropertyName = "requiresDiscountCode")]
        public bool RequiresDiscountCode { get; private set; }

        #endregion

        public CartDiscountDraft(
            LocalizedString name, 
            LocalizedString description,
            CartDiscountValue cartDiscountValue,
            CartDiscountTarget cartDiscountTarget,
            string cartPredicate,
            string sortOrder,
            bool isActive,
            DateTime? validFrom,
            DateTime? validUnitil,
            bool requiresDiscountCode)
        {
            if (name == null)
                throw new ArgumentException(nameof(name));

            if (string.IsNullOrWhiteSpace(sortOrder))
                throw new ArgumentException(nameof(sortOrder));

            if (string.IsNullOrWhiteSpace(cartPredicate))
                throw new ArgumentException(nameof(cartPredicate));

            if (cartDiscountValue == null)
                throw new ArgumentException(nameof(cartDiscountValue));

            if (cartDiscountValue.Type != CartDiscountType.GiftLineItem && cartDiscountTarget == null)
                throw new ArgumentException(nameof(cartDiscountTarget));


            Description = description;
            Name = name;
            CartPredicate = cartPredicate;
            Value = cartDiscountValue;
            Target = cartDiscountTarget;
            ValidUntil = validUnitil;
            ValidFrom = validFrom;
            IsActive = isActive;
            SortOrder = sortOrder;
            RequiresDiscountCode = requiresDiscountCode;
        }
    }
}
