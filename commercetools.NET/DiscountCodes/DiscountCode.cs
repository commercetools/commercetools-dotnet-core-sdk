using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes
{
    public class DiscountCode
    {
        #region properties

        /// <summary>
        /// The unique ID of the discount code.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The current version of the discount code.
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
        /// Discount code Predicate.The discount code can only be applied to carts that match this predicate.
        /// </summary>
        [JsonProperty(PropertyName = "cartPredicate")]
        public string CartPredicate { get; private set; }

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
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; private set; }

        [JsonProperty(PropertyName = "maxApplications")]
        public int? MaxApplications { get; set; }

        [JsonProperty(PropertyName = "maxApplicationsPerCustomer")]
        public int? MaxApplicationsPerCustomer { get; set; }

        /// <summary>
        /// The platform will generate this array from the cartPredicate. 
        /// It contains the references of all the resources that are addressed in the predicate.
        /// </summary>
        [JsonProperty(PropertyName = "references")]
        public List<Reference> References { get; private set; }

        #endregion

        public DiscountCode(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Name = new LocalizedString(data.name);
            this.Description = new LocalizedString(data.description);
            this.Code = data.code;
            this.CartDiscounts = Helper.GetListFromJsonArray<Reference>(data.cartDiscounts);
            this.CartPredicate = data.cartPredicate;
            this.IsActive = data.isActive;
            this.References = Helper.GetListFromJsonArray<Reference>(data.references);
            this.MaxApplications = data.maxApplications;
            this.MaxApplicationsPerCustomer = data.maxApplicationsPerCustomer;
        }
    }
}
