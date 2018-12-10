using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingMethodsForOrderEditAdditionalParameters : IAdditionalParameters<ShippingMethod>
    {
        [Required]
        [Country]
        public string Country { get; set; }
        public string State { get; set; }
        [Required]
        public string OrderEditId { get; set; }
    }
}
