using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class ChangeRequiresDiscountCodeUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeRequiresDiscountCode";
        [Required]
        public bool RequiresDiscountCode { get; set; }
    }
}