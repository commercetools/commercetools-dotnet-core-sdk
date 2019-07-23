using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class CartDiscountDraft : IDraft<CartDiscount>
    {
        public string Key { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public CartDiscountValue Value { get; set; }
        public string CartPredicate { get; set; }
        public CartDiscountTarget Target { get; set; }
        public string SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool RequiresDiscountCode { get; set; }
        public List<Reference> References { get; set; }
        public StackingMode StackingMode { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
