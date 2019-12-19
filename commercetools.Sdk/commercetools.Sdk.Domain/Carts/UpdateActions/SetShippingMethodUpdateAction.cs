using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetShippingMethodUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingMethod";
        public IReference<ShippingMethod> ShippingMethod { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}