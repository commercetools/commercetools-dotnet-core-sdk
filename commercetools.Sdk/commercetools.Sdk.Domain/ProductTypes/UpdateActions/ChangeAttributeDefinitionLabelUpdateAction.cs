using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeAttributeDefinitionLabelUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeLabel";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public LocalizedString Label { get; set; }
    }
}