using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class GetShippingMethodsForCartAdditionalParameters : IAdditionalParameters<ShippingMethod>
    {
        [Required]
        public string CartId { get; set; }
    }
}
