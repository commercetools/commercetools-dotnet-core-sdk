using commercetools.Sdk.Domain.Products.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeLocalizedEnumValueOrderUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeLocalizedEnumValueOrder";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public List<LocalizedEnumValue> Values { get; set; }
    }
}