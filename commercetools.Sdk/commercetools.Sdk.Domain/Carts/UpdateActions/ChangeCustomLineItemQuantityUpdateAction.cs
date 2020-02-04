using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ChangeCustomLineItemQuantityUpdateAction : CartUpdateAction
    {
        public override string Action => "changeCustomLineItemQuantity";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public long Quantity { get; set; }
    }
}
