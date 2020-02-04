using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ChangeTaxCalculationModeUpdateAction : CartUpdateAction
    {
        public override string Action => "changeTaxCalculationMode";
        [Required]
        public TaxCalculationMode TaxCalculationMode { get; set; }
    }
}