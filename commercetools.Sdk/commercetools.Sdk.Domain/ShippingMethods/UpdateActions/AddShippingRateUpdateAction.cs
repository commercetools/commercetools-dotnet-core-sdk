using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class AddShippingRateUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "addShippingRate";
        [Required]
        public ResourceIdentifier Zone { get; set; }
        [Required]
        public ShippingRateDraft ShippingRate { get; set; }
    }
}