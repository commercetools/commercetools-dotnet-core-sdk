using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class ChangeTaxCategoryUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "changeTaxCategory";
        [Required]
        public IReference<TaxCategory> TaxCategory { get; set; }
    }
}
