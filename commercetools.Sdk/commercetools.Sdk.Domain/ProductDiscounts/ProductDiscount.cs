using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    [Endpoint("product-discounts")]
    [ResourceType(ReferenceTypeId.ProductDiscount)]
    public class ProductDiscount : Resource<ProductDiscount>, IKeyReferencable<ProductDiscount>
    {
        public LocalizedString Name { get; set; }
        
        public string Key { get; set; }
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
