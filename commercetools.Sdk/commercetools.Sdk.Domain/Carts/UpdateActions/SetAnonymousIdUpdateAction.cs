namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SetAnonymousIdUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setAnonymousId";
        public string AnonymousId { get; set; }
    }
}