using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class ChangeDescriptionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeDescription";
        [Required]
        public string Description { get; set; }
    }
}
