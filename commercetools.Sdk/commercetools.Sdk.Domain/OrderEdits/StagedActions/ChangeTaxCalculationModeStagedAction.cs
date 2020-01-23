using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeTaxCalculationModeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeTaxCalculationMode";
        [Required]
        public TaxCalculationMode TaxCalculationMode { get; set; }
    }
}