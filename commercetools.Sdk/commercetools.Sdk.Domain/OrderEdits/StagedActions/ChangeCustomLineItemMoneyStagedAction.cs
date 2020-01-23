using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeCustomLineItemMoneyStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeCustomLineItemMoney";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public BaseMoney Money { get; set; }
    }
}