using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxRates
{
    public class RemoveTaxRateUpdateAction : UpdateAction<TaxRate>
    {
        public string Action => "removeTaxRate";
        [Required]
        public string TaxRateId { get; set; }
    }
}