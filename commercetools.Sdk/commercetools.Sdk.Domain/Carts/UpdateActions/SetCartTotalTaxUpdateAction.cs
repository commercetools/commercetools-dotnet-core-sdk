using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetCartTotalTaxUpdateAction : CartUpdateAction
    {
        public override string Action => "setCartTotalTax";
        [Required]
        public Money ExternalTotalGross { get; set; }
        public List<TaxPortion> ExternalTaxPortions { get; set; }
    }
}