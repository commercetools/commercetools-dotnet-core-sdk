using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingMethodsForCartAdditionalParameters : IAdditionalParameters<ShippingMethod>
    {
        [Required]
        public string CartId { get; set; }
    }
}
