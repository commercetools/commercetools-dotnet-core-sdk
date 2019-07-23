using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeNameUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}
