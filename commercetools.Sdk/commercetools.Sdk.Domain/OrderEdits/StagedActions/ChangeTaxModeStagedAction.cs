using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeTaxModeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeTaxMode";
        [Required]
        public TaxMode TaxMode { get; set; }
    }
}