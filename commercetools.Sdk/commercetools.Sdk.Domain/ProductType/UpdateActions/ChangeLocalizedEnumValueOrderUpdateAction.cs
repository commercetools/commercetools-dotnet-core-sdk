using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeLocalizedEnumValueOrderUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeLocalizedEnumValueOrder";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public List<Attributes.LocalizedEnumValue> Values { get; set; }
    }
}