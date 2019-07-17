using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeStackingModeUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeStackingMode";
        [Required]
        public StackingMode StackingMode { get; set; }
    }
}
