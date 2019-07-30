using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Zones;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class AddZoneUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "addZone";
        [Required]
        public IReference<Zone> Zone { get; set; }
    }
}
