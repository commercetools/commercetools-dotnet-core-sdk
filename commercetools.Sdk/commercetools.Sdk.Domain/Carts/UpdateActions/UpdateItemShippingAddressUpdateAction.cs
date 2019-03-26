namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class UpdateItemShippingAddressUpdateAction : UpdateAction<Cart>
    {
        public string Action => "updateItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}
