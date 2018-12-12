using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class AddAttributeDefinitionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "addAttributeDefinition";
        [Required]
        public AttributeDefinitionDraft Attribute { get; set; }
    }
}