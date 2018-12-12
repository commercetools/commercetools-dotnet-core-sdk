using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeEnumValueLabelUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changePlainEnumValueLabel";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public PlainEnumValue NewValue { get; set; }
    }
}