using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeLocalizedEnumValueLabelUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeLocalizedEnumValueLabel";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public LocalizedEnumValue NewValue { get; set; }
    }
}