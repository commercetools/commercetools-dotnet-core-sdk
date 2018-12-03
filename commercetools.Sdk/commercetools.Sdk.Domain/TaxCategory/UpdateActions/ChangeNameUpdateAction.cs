using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxRates
{
    public class ChangeNameUpdateAction : UpdateAction<TaxRate>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}