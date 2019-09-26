using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class GetMatchingProductDiscountParameters : IGetMatchingParameters<ProductDiscount>
    {
        public string ProductId { get; set; }

        public int? VariantId { get; set; }

        public bool? Staged { get; set; }

        [Required]
        public Price Price { get; set;  }
    }
}
