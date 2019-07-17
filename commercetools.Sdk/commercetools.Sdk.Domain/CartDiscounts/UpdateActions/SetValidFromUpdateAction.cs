using System;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetValidFromUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidFrom";
        public DateTime? ValidFrom { get; set; }
    }
}
