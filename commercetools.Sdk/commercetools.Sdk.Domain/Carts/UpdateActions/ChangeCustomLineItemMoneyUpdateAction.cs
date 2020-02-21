using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ChangeCustomLineItemMoneyUpdateAction : CartUpdateAction
    {
        public override string Action => "changeCustomLineItemMoney";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public BaseMoney Money { get; set; }
    }
}