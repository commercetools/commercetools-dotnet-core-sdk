namespace commercetools.Sdk.Domain.Carts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class ExternalTaxRateDraft
    {
        [Required]
        public string Name { get; set; }
        public double Amount { get; set; }
        [Required]
        [Country]
        public string Country { get; set; }
        public string State { get; set; }
        public List<SubRate> SubRates { get; set; }
        public bool IncludedInPrice { get; set; }
    }
}
