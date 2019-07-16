using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxCategories.UpdateActions
{
    public class AddTaxRateUpdateAction : UpdateAction<TaxCategory>
    {
        public string Action => "addTaxRate";
        [Required]
        public TaxRateDraft TaxRate { get; set; }
    }
}
