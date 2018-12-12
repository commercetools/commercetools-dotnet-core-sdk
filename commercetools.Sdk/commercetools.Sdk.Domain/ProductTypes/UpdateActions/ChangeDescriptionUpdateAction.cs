using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductTypes
{
    public class ChangeDescriptionUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "changeDescription";
        [Required]
        public LocalizedString Name { get; set; }
    }
}