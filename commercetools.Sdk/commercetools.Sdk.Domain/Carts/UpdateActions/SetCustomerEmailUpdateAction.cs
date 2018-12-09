namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetCustomerEmailUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomerEmail";
        public string Email { get; set; }
    }
}