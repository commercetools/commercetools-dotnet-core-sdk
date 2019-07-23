using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeAttributeDefinitionIsSearchableUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeIsSearchable";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public bool IsSearchable { get; set; }
    }
}