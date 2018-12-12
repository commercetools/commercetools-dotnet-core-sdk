using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
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