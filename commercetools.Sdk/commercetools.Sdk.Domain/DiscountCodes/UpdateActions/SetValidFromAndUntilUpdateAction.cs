using System;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetValidFromAndUntilUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setValidFromAndUntil";
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
