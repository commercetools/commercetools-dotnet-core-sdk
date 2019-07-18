using System;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetValidUntilUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setValidUntil";
        public DateTime? ValidUntil { get; set; }
    }
}
