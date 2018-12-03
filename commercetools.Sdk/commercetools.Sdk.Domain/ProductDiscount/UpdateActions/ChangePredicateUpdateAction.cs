using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class ChangePredicateUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "changePredicate";
        [Required]
        public string Predicate { get; set; }
    }
}