using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class AddLocalizableEnumValueToAttributeDefinitionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "addLocalizedEnumValue";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public Attributes.LocalizedEnumValue Value { get; set; }
    }
}