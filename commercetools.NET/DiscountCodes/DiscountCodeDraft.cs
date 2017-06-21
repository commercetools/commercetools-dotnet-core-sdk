using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes
{
    public class DiscountCodeDraft
    {
        #region properties

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        /// <summary>
        /// Cart Discount Predicate
        /// </summary>
        [JsonProperty(PropertyName = "cartPredicate")]
        public string CartPredicate { get; set; }

        /// <summary>
        /// Array of Reference to a CartDiscount.
        /// </summary>
        [JsonProperty(PropertyName = "cartDiscounts")]
        public List<Reference> CartDiscounts { get; private set; }

        /// <summary>
        /// Unique identifier of this discount code. 
        /// This value is added to the cart to enable the related cart discounts in the cart.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; private set; }

        [JsonProperty(PropertyName = "maxApplications")]
        public int? MaxApplications { get; set; }

        [JsonProperty(PropertyName = "maxApplicationsPerCustomer")]
        public int? MaxApplicationsPerCustomer { get; set; }

        #endregion

        public DiscountCodeDraft(
            string code,
            List<Reference> cartDiscounts,
            bool isActive)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            if (cartDiscounts == null || cartDiscounts.Count == 0)
                throw new ArgumentException($"{nameof(cartDiscounts)} cannot be null or empty.");


            Code = code;
            CartDiscounts = cartDiscounts;
            IsActive = isActive;
        }
    }
}