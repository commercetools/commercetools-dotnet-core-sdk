namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemTotalPriceUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemTotalPrice";
        [Required]
        public string LineItemId { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
    }
}