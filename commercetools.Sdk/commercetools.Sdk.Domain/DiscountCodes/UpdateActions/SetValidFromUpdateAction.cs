using System;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetValidFromUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setValidFrom";
        public DateTime? ValidFrom { get; set; }
    }
}
