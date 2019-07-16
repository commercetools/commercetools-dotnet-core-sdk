using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxCategories.UpdateActions
{
    public class ReplaceTaxRateUpdateAction : UpdateAction<TaxCategory>
    {
        public string Action => "replaceTaxRate";
        public string TaxRateId { get; set; }
        [Required]
        public TaxRateDraft TaxRate { get; set; }
    }
}
