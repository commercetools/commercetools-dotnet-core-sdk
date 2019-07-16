using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.TaxCategories.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<TaxCategory>
    {
        public string Action => "changeName";
        [Required]
        public string Name { get; set; }
    }
}
