using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class RemoveEnumValuesFromAttributeDefinitionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "removeEnumValues";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public List<string> Keys { get; set; }
    }
}