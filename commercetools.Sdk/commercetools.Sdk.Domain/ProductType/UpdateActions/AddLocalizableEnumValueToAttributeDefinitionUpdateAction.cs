using commercetools.Sdk.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
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