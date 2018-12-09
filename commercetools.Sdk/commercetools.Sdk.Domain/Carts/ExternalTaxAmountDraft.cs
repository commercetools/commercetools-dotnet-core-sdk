namespace commercetools.Sdk.Domain.Carts
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalTaxAmountDraft
    {
        [Required]
        public Money TotalGross { get; set; }
        [Required]
        public ExternalTaxRateDraft TaxRate { get; set; }
    }
}