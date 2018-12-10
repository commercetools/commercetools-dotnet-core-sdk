using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingMethodsForLocationAdditionalParameters : IAdditionalParameters<ShippingMethod>
    {
        [Required]
        [Country]
        public string Country { get; set; }
        public string State { get; set; }
        [Currency]
        public string Currency { get; set; }
    }
}
