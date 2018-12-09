namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeCustomLineItemMoneyUpdateAction : UpdateAction<Cart>
    {
        public string Action => "changeCustomLineItemMoney";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public BaseMoney Money { get; set; }
    }
}