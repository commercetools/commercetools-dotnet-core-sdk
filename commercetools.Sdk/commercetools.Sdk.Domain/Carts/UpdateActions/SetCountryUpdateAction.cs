namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetCountryUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCountry";
        [Country]
        public string Country { get; set; }
    }
}