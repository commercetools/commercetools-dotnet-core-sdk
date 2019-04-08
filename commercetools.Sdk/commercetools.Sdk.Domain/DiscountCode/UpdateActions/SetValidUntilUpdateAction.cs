using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetValidUntilUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setValidUntil";
        public DateTime? ValidUntil { get; set; }
    }
}
