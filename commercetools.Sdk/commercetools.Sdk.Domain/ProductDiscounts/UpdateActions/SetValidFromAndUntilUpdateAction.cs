using System;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class SetValidFromAndUntilUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setValidFromAndUntil";
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
