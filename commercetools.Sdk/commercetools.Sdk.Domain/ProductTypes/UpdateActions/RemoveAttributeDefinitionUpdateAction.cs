using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class RemoveAttributeDefinitionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "removeAttributeDefinition";
        [Required]
        public string Name { get; set; }
    }
}