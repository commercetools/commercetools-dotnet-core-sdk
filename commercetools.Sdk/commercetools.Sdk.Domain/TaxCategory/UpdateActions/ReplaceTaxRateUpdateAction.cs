using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxRates
{
    public class ReplaceTaxRateUpdateAction : UpdateAction<TaxRate>
    {
        public string Action => "replaceTaxRate";
        public string TaxRateId { get; set; }
        [Required]
        public TaxRateDraft TaxRate { get; set; }
    }
}