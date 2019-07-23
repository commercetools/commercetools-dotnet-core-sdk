using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeIsActiveUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeIsActive";
        [Required]
        public bool IsActive { get; set; }
    }
}
