using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetCartTotalTaxUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCartTotalTax";
        [Required]
        public Money ExternalTotalGross { get; set; }
        public List<TaxPortion> ExternalTaxPortions { get; set; }
    }
}