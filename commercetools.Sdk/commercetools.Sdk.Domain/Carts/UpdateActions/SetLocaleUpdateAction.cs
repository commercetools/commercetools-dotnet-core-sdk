namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetLocaleUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLocale";
        [Language]
        public string Locale { get; set; }
    }
}