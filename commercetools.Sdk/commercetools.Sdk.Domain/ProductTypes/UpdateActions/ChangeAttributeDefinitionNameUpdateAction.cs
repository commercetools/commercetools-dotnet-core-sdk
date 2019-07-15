using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeAttributeDefinitionNameUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeAttributeName";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public string NewAttributeName { get; set; }
    }
}