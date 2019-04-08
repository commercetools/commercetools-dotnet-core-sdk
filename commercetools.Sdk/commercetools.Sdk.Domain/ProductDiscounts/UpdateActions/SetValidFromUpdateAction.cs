using System;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class SetValidFromUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setValidFrom";
        public DateTime? ValidFrom { get; set; }
    }
}
