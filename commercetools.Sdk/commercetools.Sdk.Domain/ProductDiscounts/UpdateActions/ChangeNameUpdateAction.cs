using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}