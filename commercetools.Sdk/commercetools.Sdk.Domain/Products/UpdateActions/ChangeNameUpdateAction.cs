using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
        public bool Staged { get; set; }
    }
}