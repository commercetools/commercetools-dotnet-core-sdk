using commercetools.Sdk.Domain.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeLocalizedEnumValueLabelUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeLocalizedEnumValueLabel";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public Attributes.LocalizedEnumValue NewValue { get; set; }
    }
}