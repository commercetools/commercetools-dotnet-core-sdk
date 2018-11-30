using commercetools.Sdk.Domain.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangePlainEnumValueOrderUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changePlainEnumValueOrder";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public List<PlainEnumValue> Values { get; set; }
    }
}