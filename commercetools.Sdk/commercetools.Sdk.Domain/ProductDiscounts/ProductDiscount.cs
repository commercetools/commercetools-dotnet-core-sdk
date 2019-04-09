using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    [Endpoint("product-discounts")]
    [ResourceType(ReferenceTypeId.ProductDiscount)]
    public class ProductDiscount
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public ProductDiscountValue Value { get; set; }
        public string Predicate { get; set; }
        public string SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<Reference> References { get; set; }
    }
}
