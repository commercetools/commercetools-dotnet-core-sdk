namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemoveLineItemUpdateAction : UpdateAction<Cart>
    {
        public string Action => "removeLineItem";
        [Required]
        public string LineItemId { get; set; }
        public long Quantity { get; set; }
        public BaseMoney ExternalPrice { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
        public ItemShippingDetailsDraft ShippingDetailsToRemove { get; set; }
    }
}
