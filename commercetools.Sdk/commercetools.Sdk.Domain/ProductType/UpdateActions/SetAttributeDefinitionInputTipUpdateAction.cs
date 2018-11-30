using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class SetAttributeDefinitionInputTipUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "setInputTip";
        [Required]
        public string AttributeName { get; set; }
        public LocalizedString InputTip { get; set; }
    }
}