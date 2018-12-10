using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class ChangeTaxCategoryUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "changeTaxCategory";
        [Required]
        public ResourceIdentifier TaxCategory { get; set; }
    }
}