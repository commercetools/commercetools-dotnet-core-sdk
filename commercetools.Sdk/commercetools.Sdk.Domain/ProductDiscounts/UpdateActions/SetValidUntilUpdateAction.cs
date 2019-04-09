using System;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class SetValidUntilUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setValidUntil";
        public DateTime? ValidUntil { get; set; }
    }
}
