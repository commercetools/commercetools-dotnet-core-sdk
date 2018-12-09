namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemShippingDetailsUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemShippingDetails";
        [Required]
        public string LineItemId { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}