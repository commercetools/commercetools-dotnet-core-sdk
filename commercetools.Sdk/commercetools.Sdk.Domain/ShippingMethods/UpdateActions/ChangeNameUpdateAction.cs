using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    [Obsolete("Use SetLocalizedNameUpdateAction instead")]
    public class ChangeNameUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "changeName";
        [Required]
        public string Name { get; set; }
    }
}
