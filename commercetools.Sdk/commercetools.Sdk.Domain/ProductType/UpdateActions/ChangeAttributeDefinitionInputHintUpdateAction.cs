using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeAttributeDefinitionInputHintUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeInputHint";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public TextInputHint NewValue { get; set; }
    }
}