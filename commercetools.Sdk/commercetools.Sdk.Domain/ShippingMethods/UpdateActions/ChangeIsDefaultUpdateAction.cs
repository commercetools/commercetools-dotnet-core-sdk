using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class ChangeIsDefaultUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "changeIsDefault";
        [Required]
        public bool IsDefault { get; set; }
    }
}