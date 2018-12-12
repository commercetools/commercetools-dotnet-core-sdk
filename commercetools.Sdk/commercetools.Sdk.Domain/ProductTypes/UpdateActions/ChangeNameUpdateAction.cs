using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeNameUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}