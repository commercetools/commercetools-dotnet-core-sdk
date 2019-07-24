using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "changeName";
        [Required]
        public string Name { get; set; }
    }
}
