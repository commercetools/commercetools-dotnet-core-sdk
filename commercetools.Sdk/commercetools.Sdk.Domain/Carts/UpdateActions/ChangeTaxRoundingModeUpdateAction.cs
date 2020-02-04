using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ChangeTaxRoundingModeUpdateAction : CartUpdateAction
    {
        public override string Action => "changeTaxRoundingMode";
        [Required]
        public RoundingMode TaxRoundingMode { get; set; }
    }
}