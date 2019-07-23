using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    [Endpoint("discount-codes")]
    [ResourceType(ReferenceTypeId.DiscountCode)]
    public class DiscountCode : Resource<DiscountCode>
    {
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public string Code { get; set; }
        public List<Reference<CartDiscount>> CartDiscounts { get; set; }
        public string CartPredicate { get; set; }
        public List<string> Groups { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<Reference> References { get; set; }
        public long MaxApplications { get; set; }
        public long MaxApplicationsPerCustomer { get; set; }
        public CustomFields Custom { get; set; }

    }
}
