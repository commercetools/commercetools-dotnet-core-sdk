namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeTaxRoundingModeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "changeTaxRoundingMode";
        [Required]
        public RoundingMode TaxRoundingMode { get; set; }
    }
}