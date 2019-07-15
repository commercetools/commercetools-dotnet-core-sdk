using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
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