namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeLineItemQuantityUpdateAction : UpdateAction<Cart>
    {
        public string Action => "changeLineItemQuantity";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public double Quantity { get; set; }
        public BaseMoney ExternalPrice { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
    }
}