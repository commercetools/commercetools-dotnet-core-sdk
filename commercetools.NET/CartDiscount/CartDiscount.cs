using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscount
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

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "validFrom")]
        public DateTime? ValidFrom { get; private set; }

        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; private set; }

        /// <summary>
        /// States whether the discount can only be used in a connection with a DiscountCode.
        /// </summary>
        [JsonProperty(PropertyName = "requiresDiscountCode")]
        public bool RequiresDiscountCode { get; private set; }

        [JsonProperty(PropertyName = "references")]
        public List<Reference> References { get; private set; }
        #endregion
    }
}
