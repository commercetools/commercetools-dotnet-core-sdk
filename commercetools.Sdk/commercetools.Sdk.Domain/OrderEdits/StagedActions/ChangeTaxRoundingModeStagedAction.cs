using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeTaxRoundingModeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeTaxRoundingMode";
        [Required]
        public RoundingMode TaxRoundingMode { get; set; }
    }
}