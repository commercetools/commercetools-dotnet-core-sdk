using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxRates
{
    public class AddTaxRateUpdateAction : UpdateAction<TaxRate>
    {
        public string Action => "addTaxRate";
        [Required]
        public TaxRateDraft TaxRate { get; set; }
    }
}