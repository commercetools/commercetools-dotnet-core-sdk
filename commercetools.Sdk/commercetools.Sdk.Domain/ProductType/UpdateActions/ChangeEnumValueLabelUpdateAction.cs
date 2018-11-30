using commercetools.Sdk.Domain.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
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