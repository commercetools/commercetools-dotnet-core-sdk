namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetBillingAddressUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setBillingAddress";
        public Address Address { get; set; }
    }
}