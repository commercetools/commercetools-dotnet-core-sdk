namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetShippingRateInputUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingRateInput";
        public ShippingRateInputDraft ShippingRateInput { get; set; }
    }
}