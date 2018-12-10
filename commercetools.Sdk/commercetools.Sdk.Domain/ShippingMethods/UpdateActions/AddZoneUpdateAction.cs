using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class AddZoneUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "addZone";
        [Required]
        public ResourceIdentifier Zone { get; set; }
    }
}