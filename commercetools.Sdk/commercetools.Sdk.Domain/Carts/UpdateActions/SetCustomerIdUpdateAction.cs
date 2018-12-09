namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetCustomerIdUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomerId";
        public string CustomerId { get; set; }
    }
}