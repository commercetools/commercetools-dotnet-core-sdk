namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class RemoveItemShippingAddressUpdateAction : UpdateAction<Cart>
    {
        public string Action => "removeItemShippingAddress";
        [Required]
        public string AddressKey { get; set; }
    }
}