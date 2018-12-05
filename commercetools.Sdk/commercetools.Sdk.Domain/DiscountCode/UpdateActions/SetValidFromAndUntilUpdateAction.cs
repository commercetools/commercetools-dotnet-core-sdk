using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetValidFromAndUntilUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setValidFromAndUntil";
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}