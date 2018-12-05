using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetValidFromUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setValidFrom";
        public DateTime ValidFrom { get; set; }
    }
}