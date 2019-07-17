using System;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetValidFromAndUntilUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidFromAndUntil";
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
