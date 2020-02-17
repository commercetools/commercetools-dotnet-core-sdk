using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class AddDiscountCodeUpdateAction : CartUpdateAction
    {
        public override string Action => "addDiscountCode";
        [Required]
        public string Code { get; set; }
    }
}