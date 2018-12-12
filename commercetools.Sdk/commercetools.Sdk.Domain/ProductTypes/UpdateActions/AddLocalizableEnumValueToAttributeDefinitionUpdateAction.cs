using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class AddPlainEnumValueToAttributeDefinitionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "addPlainEnumValue";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public PlainEnumValue Value { get; set; }
    }
}