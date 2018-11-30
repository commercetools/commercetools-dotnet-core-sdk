using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeAttributeDefinitionAttributeConstraintUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeAttributeConstraint";
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public AttributeConstraint NewValue { get; set; }
    }
}