using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeAttributeDefinitionsOrderUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeAttributeOrderByName";
        [Required]
        public List<string> AttributeNames { get; set; }
    }
}
