using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeValueUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeValue";
        [Required]
        public CartDiscountValue Value { get; set; }
    }
}
