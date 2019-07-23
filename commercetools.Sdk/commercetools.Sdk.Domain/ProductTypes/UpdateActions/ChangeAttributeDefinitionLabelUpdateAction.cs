using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
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