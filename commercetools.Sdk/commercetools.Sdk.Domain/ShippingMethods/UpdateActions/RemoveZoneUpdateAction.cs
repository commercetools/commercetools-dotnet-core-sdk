using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class RemoveZoneUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "removeZone";
        [Required]
        public ResourceIdentifier Zone { get; set; }
    }
}