using System;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetValidUntilUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setValidUntil";
        public DateTime? ValidUntil { get; set; }
    }
}
