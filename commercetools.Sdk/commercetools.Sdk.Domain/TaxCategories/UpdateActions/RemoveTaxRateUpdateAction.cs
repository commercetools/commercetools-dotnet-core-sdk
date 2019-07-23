using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxCategories.UpdateActions
{
    public class RemoveTaxRateUpdateAction : UpdateAction<TaxCategory>
    {
        public string Action => "removeTaxRate";
        [Required]
        public string TaxRateId { get; set; }
    }
}
