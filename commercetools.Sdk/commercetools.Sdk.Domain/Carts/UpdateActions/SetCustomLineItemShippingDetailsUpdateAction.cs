namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemShippingDetailsUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomLineItemShippingDetails";
        [Required]
        public string CustomLineItemId { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}