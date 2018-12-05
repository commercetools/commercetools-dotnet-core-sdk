using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class DiscountCodeDraft
    {
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public List<Reference<CartDiscount>> CartDiscounts { get; set; }
        public string CartPredicate { get; set; }
        public List<string> Groups { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<Reference> References { get; set; }
        public double MaxApplications { get; set; }
        public double MaxApplicationsPerCustomer { get; set; }
        public CustomFields Custom { get; set; }
    }
}
