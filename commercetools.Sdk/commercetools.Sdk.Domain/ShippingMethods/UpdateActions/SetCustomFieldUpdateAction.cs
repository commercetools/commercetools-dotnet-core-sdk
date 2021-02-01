using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetCustomFieldUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}