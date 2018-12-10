using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class RemoveShippingRateUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "removeShippingRate";
        [Required]
        public ResourceIdentifier Zone { get; set; }
        [Required]
        public ShippingRateDraft ShippingRate { get; set; }
    }
}