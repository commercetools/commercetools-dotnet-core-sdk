using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [Endpoint("cart-discounts")]
    [ResourceType(ReferenceTypeId.CartDiscount)]
    public class CartDiscount
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public CartDiscountValue Value { get; set; }
        public string CartPredicate { get; set; }
        public CartDiscountTarget Target { get; set; }
        public string SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool RequiresDiscountCode { get; set; }
        public List<Reference> References { get; set; }
        public StackingMode StackingMode { get; set; }
        public CustomFields Custom { get; set; }
    }
}
