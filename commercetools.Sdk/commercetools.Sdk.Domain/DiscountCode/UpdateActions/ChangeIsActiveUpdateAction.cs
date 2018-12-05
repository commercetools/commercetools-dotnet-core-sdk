using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class ChangeIsActiveUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "changeIsActive";
        [Required]
        public bool IsActive { get; set; }
    }
}
