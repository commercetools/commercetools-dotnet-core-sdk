namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeCustomLineItemQuantityUpdateAction : UpdateAction<Cart>
    {
        public string Action => "changeCustomLineItemQuantity";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public long Quantity { get; set; }
    }
}
