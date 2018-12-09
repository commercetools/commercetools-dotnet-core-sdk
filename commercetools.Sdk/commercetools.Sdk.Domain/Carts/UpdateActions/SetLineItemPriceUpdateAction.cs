namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemPriceUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemPrice";
        [Required]
        public string LineItemId { get; set; }
        public BaseMoney ExternalPrice { get; set; }
    }
}