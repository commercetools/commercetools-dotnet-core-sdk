using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class AddLocalizableEnumValueToAttributeDefinitionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "addLocalizedEnumValue";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public LocalizedEnumValue Value { get; set; }
    }
}