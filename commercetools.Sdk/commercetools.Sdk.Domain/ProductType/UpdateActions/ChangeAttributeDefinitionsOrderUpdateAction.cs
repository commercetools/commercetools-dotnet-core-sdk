using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeAttributeDefinitionsOrderUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeAttributeOrder";
        [Required]
        public List<AttributeDefinition> Attributes { get; set; }
    }
}