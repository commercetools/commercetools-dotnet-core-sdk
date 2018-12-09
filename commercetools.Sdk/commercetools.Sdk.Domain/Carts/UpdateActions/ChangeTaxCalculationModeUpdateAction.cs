namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeTaxCalculationModeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "changeTaxCalculationMode";
        [Required]
        public TaxCalculationMode TaxCalculationMode { get; set; }
    }
}