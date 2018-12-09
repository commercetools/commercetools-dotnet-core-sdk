namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetShippingMethodTaxAmountUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingMethodTaxAmount";
        public ExternalTaxAmountDraft ExternalTaxAmount { get; set; }
    }
}