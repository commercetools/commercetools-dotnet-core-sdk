using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeName";
        [Required]
        public string Name { get; set; }
    }
}