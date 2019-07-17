using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeTargetUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeTarget";
        [Required]
        public CartDiscountTarget Target { get; set; }
    }
}
