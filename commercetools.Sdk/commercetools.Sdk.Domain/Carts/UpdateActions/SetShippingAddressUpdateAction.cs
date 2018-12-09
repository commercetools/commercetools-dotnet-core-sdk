namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetShippingAddressUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingAddress";
        public Address Address { get; set; }
    }
}