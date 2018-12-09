namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetShippingMethodTaxRateUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingMethodTaxRate";
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}